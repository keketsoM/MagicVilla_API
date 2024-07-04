﻿using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;


namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseServices
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClient, ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
            this.responseModel = new APIResponse();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
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
                if (withBearer == true && _tokenProvider.GetToken() != null)
                {
                    var token = _tokenProvider.GetToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }

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
                HttpResponseMessage apiResponse = null;
                if (!string.IsNullOrEmpty(apiRequest.token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.token);
                }
                apiResponse = await client.SendAsync(message);

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
    }
}
