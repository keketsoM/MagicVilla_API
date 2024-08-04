using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
        Task<T> LogoutAsync<T>(TokenDto tokenDto);
    }
}
