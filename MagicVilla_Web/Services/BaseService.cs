using AutoMapper.Internal;
using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;


namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseServices
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IApiMessageRequestBuilder _apiMessageRequestBuilder;
        protected readonly string VillaApiUrl;
        public BaseService(IHttpClientFactory httpClient, ITokenProvider tokenProvider,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IApiMessageRequestBuilder apiMessageRequestBuilder)
        {
            _apiMessageRequestBuilder = apiMessageRequestBuilder;
            _tokenProvider = tokenProvider;
            this.responseModel = new APIResponse();
            this.httpClient = httpClient;
            VillaApiUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");

                var messageFactory = () =>
               {
                   return _apiMessageRequestBuilder.Build(apiRequest);
               };

                HttpResponseMessage httpResponseMessage = null;

                httpResponseMessage = await SendWithRefreshTokenAsync(client, messageFactory, withBearer);
                APIResponse FinalApiResponse = new APIResponse()
                {
                    IsSuccess = false
                };

                try
                {
                    switch (httpResponseMessage.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            FinalApiResponse.ErrorMessages = new List<string>() { "BadRequest" };
                            break;
                        case HttpStatusCode.NotFound:
                            FinalApiResponse.ErrorMessages = new List<string>() { "NotFound" };
                            break;
                        case HttpStatusCode.Unauthorized:
                            FinalApiResponse.ErrorMessages = new List<string>() { "Unauthorized" };
                            break;
                        case HttpStatusCode.Forbidden:
                            FinalApiResponse.ErrorMessages = new List<string>() { "Access Denied" };
                            break;
                        case HttpStatusCode.InternalServerError:
                            FinalApiResponse.ErrorMessages = new List<string>() { "InternalServerError" };
                            break;
                        default:
                            var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                            FinalApiResponse.IsSuccess = true;
                            FinalApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                            break;
                    }




                }
                catch (Exception e)
                {
                    FinalApiResponse.ErrorMessages = new List<string>() { "Error Encounted", e.Message.ToString() };
                }
                var res = JsonConvert.SerializeObject(FinalApiResponse);
                var returnObj = JsonConvert.DeserializeObject<T>(res);
                return returnObj;
            }
            catch (AuthException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient, Func<HttpRequestMessage> httpRequestMessageFactory, bool withBearer = true)
        {
            if (!withBearer)
            {
                return await httpClient.SendAsync(httpRequestMessageFactory());
            }
            else
            {
                TokenDto tokenDto = _tokenProvider.GetToken();
                if (tokenDto != null && !string.IsNullOrEmpty(tokenDto.AccessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.AccessToken);
                }

                try
                {
                    var response = await httpClient.SendAsync(httpRequestMessageFactory());
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                    // If this fails then we can pass refresh token!
                    if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //Generate new token from refresh token / sign in with that new token and then retry
                        await InvokeRefreshTokenEndpoint(httpClient, tokenDto.AccessToken, tokenDto.RefreshToken);
                        response = await httpClient.SendAsync(httpRequestMessageFactory());
                        return response;
                    }

                    return response;
                }
                catch (AuthException)
                {
                    throw;
                }
                catch (HttpRequestException httpRequestException)
                {
                    if (httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await InvokeRefreshTokenEndpoint(httpClient, tokenDto.AccessToken, tokenDto.RefreshToken);
                        return await httpClient.SendAsync(httpRequestMessageFactory());
                    }
                    throw;
                }
            }
        }
        private async Task InvokeRefreshTokenEndpoint(HttpClient client, string existingAccessToken, string existingRefreshToken)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri($"{VillaApiUrl}/api/{SD.CurrentVersion}/UsersAuth/RefreshToken");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                AccessToken = existingAccessToken,
                RefreshToken = existingRefreshToken
            }), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(content);

            if (apiResponse?.IsSuccess != true)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync();
                _tokenProvider.RemoveToken();
                throw new AuthException();
            }
            else
            {
                var tokenDataString = JsonConvert.SerializeObject(apiResponse.Result);
                var tokenDto = JsonConvert.DeserializeObject<TokenDto>(tokenDataString);
                if (tokenDto != null && !string.IsNullOrEmpty(tokenDto.AccessToken))
                {
                    // New method to sign in with the new token that we received
                    await SignInWithNewTokens(tokenDto);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.AccessToken);

                }
            }
        }
        private async Task SignInWithNewTokens(TokenDto tokenDto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenDto.AccessToken);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            _tokenProvider.SetToken(tokenDto);
        }
    }
}
