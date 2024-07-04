using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbcontext _dbcontext;
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(ApplicationDbcontext dbcontext, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {

            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _dbcontext = dbcontext;
            secretKey = configuration.GetValue<string>("Apisettings:Secret");
            _roleManager = roleManager;

        }
        public bool IsUniqueUser(string username)
        {
            var user = _dbcontext.applicationUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenDto> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _dbcontext.applicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            var result = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null)
            {
                return new TokenDto()
                {
                    AccessToken = "",

                };
            }
            //generate JWT token
            var role = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,role.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha512Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            TokenDto tokenDto = new TokenDto()
            {
                AccessToken = tokenHandler.WriteToken(token),


            };
            return tokenDto;
        }

        public async Task<UserDto> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                Name = registerationRequestDTO.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(registerationRequestDTO.Role).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registerationRequestDTO.Role));

                    }
                    await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = _dbcontext.applicationUsers.FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);

                    UserDto userDto = new UserDto()
                    {
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        Username = userToReturn.UserName
                    };
                    return userDto;
                }
            }
            catch (Exception e)
            {
                //var exceptionResponse = _response.ErrorMessages.Add(ex.Message);
                Console.WriteLine(e.Message);
            };
            return new UserDto();
        }
    }
}
