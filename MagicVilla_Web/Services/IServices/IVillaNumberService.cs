using MagicVilla_Web.Model.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService
    {

        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberDtoCreate dto);
        Task<T> UpdateAsync<T>(VillaNumberDtoUpdate dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
