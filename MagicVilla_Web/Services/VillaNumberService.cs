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

        public Task<T> CreateAsync<T>(VillaNumberDtoCreate dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI",
                Data = dto,
                token = token
            });
        }

        public Task<T> DeleteAsync<T>(int VillaNo, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/" + VillaNo,
                token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI",
                token = token

            });
        }

        public Task<T> GetAsync<T>(int VillaNo, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/" + VillaNo,
                token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberDtoUpdate dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = villaNumberUrl + $"/api/{SD.CurrentVersion}/VillaNumberAPI/" + dto.VillaNo,
                Data = dto,
                token = token
            });
        }
    }
}
