using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private string villaUrl;
        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaDtoCreate dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI",
                Data = dto,
                token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/" + id,
                token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI",
                token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/" + id,
                token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaDtoUpdate dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/" + dto.Id,
                Data = dto,
                token = token
            });
        }
    }
}
