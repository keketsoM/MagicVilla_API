using WebApi_test.Model;
using WebApi_test.Model.Dto;

namespace WebApi_test.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDto> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
