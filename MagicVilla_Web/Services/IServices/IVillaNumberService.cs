﻿using MagicVilla_Web.Model.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService
    {

        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VillaNumberDtoCreate dto, string token);
        Task<T> UpdateAsync<T>(VillaNumberDtoUpdate dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
