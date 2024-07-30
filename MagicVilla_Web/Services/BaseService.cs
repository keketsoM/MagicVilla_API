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
        protected readonly string VillaApiUrl;
        public BaseService(IHttpClientFactory httpClient, ITokenProvider tokenProvider,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
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
                   HttpRequestMessage message = new HttpRequestMessage();
                   if (apiRequest.ContentType == SD.ContentType.MultiPartFormData)
                   {
                       message.Headers.Add("Accept", "*/*");
                   }
                   else
                   {
                       message.Headers.Add("Accept", "application/json");
                   }

                   message.RequestUri = new Uri(apiRequest.Url);
                   //if (withBearer == true && _tokenProvider.GetToken() != null)
                   //{
                   //    var token = _tokenProvider.GetToken();
                   //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                   //}

                   if (apiRequest.ContentType == SD.ContentType.MultiPartFormData)
                   {
                       var content = new MultipartFormDataContent();

                       foreach (var prop in apiRequest.Data.GetType().GetProperties())
                       {
                           var value = prop.GetValue(apiRequest.Data);
                           if (value is FormFile)
                           {
                               var file = (FormFile)value;
                               if (file != null)
                               {
                                   content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                               }

                           }
                           else
                           {
                               content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                           }
                       }
                       message.Content = content;

                   }
                   else
                   {
                       message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),

                         Encoding.UTF8, "application/json");
                   }
                   switch (apiRequest.ApiType)
                   {
                       case SD.ApiType.POST:
                           message.Method = HttpMethod.Post;
                           break;
                       case SD.ApiType.PUT:
                           message.Method = HttpMethod.Put;
                           break;
                       case SD.ApiType.DELETE:
                           message.Method = HttpMethod.Delete;
                           break;
                       default:
                           message.Method = HttpMethod.Get;
                           break;
                   }

                   return message;
               };

                HttpResponseMessage apiResponse = null;

                apiResponse = await SendWithRefreshTokenAsync(client, messageFactory, withBearer);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (ApiResponse != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;

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
