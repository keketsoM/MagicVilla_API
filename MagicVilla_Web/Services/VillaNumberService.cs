using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private IHttpClientFactory _httpClientsFactory;
        private string villaNumberUrl;
        public VillaNumberService(IHttpClientFactory httpClientsFactory, IConfiguration configuration) : base(httpClientsFactory)
        {

            _httpClientsFactory = httpClientsFactory;
            villaNumberUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public Task<T> CreateAsync<T>(VillaNumberDtoCreate dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = villaNumberUrl + "/api/VillaNumberAPI",
                Data = dto
            });
        }

        public Task<T> DeleteAsync<T>(int VillaNo)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaNumberUrl + "/api/VillaNumberAPI/" + VillaNo,
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaNumberUrl + "/api/VillaNumberAPI",

            });
        }

        public Task<T> GetAsync<T>(int VillaNo)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaNumberUrl + "/api/VillaNumberAPI/" + VillaNo,
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberDtoUpdate dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = villaNumberUrl + "/api/VillaNumberAPI/" + dto.VillaNo,
                Data = dto
            });
        }
    }
}
