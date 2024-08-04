using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBaseServices _baseService;
        private string AuthUrl;
        public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IBaseServices baseServices)
        {
            _httpClientFactory = httpClientFactory;
            _baseService = baseServices;
            AuthUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO objToCreate)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = AuthUrl + $"/api/{SD.CurrentVersion}/UsersAuth/Login",
                Data = objToCreate
            }, withBearer: false);
        }

        public async Task<T> LogoutAsync<T>(TokenDto tokenDto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = AuthUrl + $"/api/{SD.CurrentVersion}/UsersAuth/Revoke",
                Data = tokenDto
            }, withBearer: false);
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = AuthUrl + $"/api/{SD.CurrentVersion}/UsersAuth/Register",
                Data = objToCreate
            }, withBearer: false);
        }
    }
}
