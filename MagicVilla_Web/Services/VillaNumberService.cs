using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : IVillaNumberService
    {
        private IHttpClientFactory _httpClientsFactory;
        private string villaNumberUrl;
        private readonly IBaseServices _baseService;
        public VillaNumberService(IHttpClientFactory httpClientsFactory, IConfiguration configuration, IBaseServices baseServices)
        {
            _baseService = baseServices;
            _httpClientsFactory = httpClientsFactory;
            villaNumberUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public async Task<T> CreateAsync<T>(VillaNumberDtoCreate dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI",
                Data = dto,

            });
        }

        public async Task<T> DeleteAsync<T>(int VillaNo)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/" + VillaNo,

            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI",


            });
        }

        public async Task<T> GetAsync<T>(int VillaNo)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/" + VillaNo,

            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberDtoUpdate dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/" + dto.VillaNo,
                Data = dto,

            });
        }
    }
}
