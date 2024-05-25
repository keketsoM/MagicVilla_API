using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Model.Dto;
using WebApi_test.Repository.IRepository;

namespace WebApi_test.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbcontext _dbcontext;
        private string secretKey;
        public UserRepository(ApplicationDbcontext dbcontext, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            secretKey = configuration.GetValue<string>("Apisettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _dbcontext.localUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _dbcontext.localUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && u.Password.ToLower() == loginRequestDTO.Password.ToLower()
            );
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    LocalUser = null
                };
            }
            //generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha512Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                LocalUser = user,
            };
            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser user = new LocalUser()
            {
                UserName = registerationRequestDTO.UserName,
                Password = registerationRequestDTO.Password,
                Name = registerationRequestDTO.Name,
                Role = registerationRequestDTO.Role,


            };
            await _dbcontext.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
