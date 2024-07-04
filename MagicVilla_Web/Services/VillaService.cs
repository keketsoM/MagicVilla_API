using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private string villaUrl;
        private readonly IBaseServices _baseService;
        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IBaseServices baseServices)
        {
            _baseService = baseServices;
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaDtoCreate dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI",
                Data = dto,

                ContentType = SD.ContentType.MultiPartFormData
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/" + id,

            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI",


            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/" + id,

            });
        }

        public async Task<T> UpdateAsync<T>(VillaDtoUpdate dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = villaUrl + $"/api/{SD.CurrentVersion}/VillaAPI/" + dto.Id,
                Data = dto,

                ContentType = SD.ContentType.MultiPartFormData
            });
        }
    }
}
