using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string AuthUrl;
        public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            AuthUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public Task<T> LoginAsync<T>(LoginRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = AuthUrl + "/api/v1/UsersAuth/Login",
                Data = objToCreate
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = AuthUrl + "/api/v1/UsersAuth/Register",
                Data = objToCreate
            });
        }
    }
}
