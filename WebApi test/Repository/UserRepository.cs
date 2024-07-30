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
            var jwtTokenId = "JTI" + Guid.NewGuid();
            var accessToken = await GenerateAccessToken(user, jwtTokenId);
            var refreshToken = await CreateNewRefreshToken(user.Id, jwtTokenId);
            TokenDto tokenDto = new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken

            };
            return tokenDto;
        }
        public async Task<TokenDto> RefreshAccessToken(TokenDto tokenDto)
        {
            //find an existing refresh token
            var existingRefreshToken = await _dbcontext.refreshTokens.FirstOrDefaultAsync(x => x.Refresh_Token == tokenDto.RefreshToken);
            if (existingRefreshToken == null)
            {
                return new TokenDto();
            }

            // compare data from existing refresh token and acess token provided and if there is any mismatch then consider it as invalid
            var accessTokenData = GetAccessTokenData(tokenDto.AccessToken);
            if (!accessTokenData.isSuccessful || accessTokenData.userId != existingRefreshToken.UserId || accessTokenData.tokenId != existingRefreshToken.JwtTokenId)
            {
                existingRefreshToken.IsValid = false;
                await _dbcontext.SaveChangesAsync();
                return new TokenDto();

            }
            //when someone tries to use not valid refresh token, fraud possible
            if (!existingRefreshToken.IsValid)
            {
                var chainRecords = await _dbcontext.refreshTokens.Where(x => x.JwtTokenId == existingRefreshToken.JwtTokenId && x.Refresh_Token == existingRefreshToken.Refresh_Token).ExecuteUpdateAsync(u => u.SetProperty(RefreshToken => RefreshToken.IsValid, false));
                //foreach (var record in chainRecords)
                //{
                //    record.IsValid = false;
                //}
                //_dbcontext.UpdateRange(chainRecords);
                //await _dbcontext.SaveChangesAsync();
                // return new TokenDto();
            }
            // if just expired then match as invalid and return empty
            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                existingRefreshToken.IsValid = false;
                await _dbcontext.SaveChangesAsync();
                return new TokenDto();
            }
            // replace old refresh token with new one with updated expiry date
            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            // revoke existing refresh token
            existingRefreshToken.IsValid = false;
            await _dbcontext.SaveChangesAsync();
            //geneerate new access token

            var applicationUser = await _dbcontext.applicationUsers.FirstOrDefaultAsync(u => u.Id == existingRefreshToken.UserId);
            if (applicationUser == null)
            {
                return new TokenDto();
            }

            return new TokenDto()
            {

                AccessToken = await GenerateAccessToken(applicationUser, existingRefreshToken.JwtTokenId),
                RefreshToken = newRefreshToken
            };

        }
        private async Task<string> CreateNewRefreshToken(string userId, string tokenId)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                IsValid = true,
                JwtTokenId = tokenId,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid()
            };

            await _dbcontext.refreshTokens.AddAsync(refreshToken);
            await _dbcontext.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }


        private (bool isSuccessful, string userId, string tokenId) GetAccessTokenData(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(accessToken);
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                var jwtTokenId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                return (true, userId, jwtTokenId);

            }
            catch
            {
                return (false, null, null);
            }

        }
        private async Task<string> GenerateAccessToken(ApplicationUser user, string JwtTokenId)
        {
            //if user was found generate JWT token
            var role = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,role.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, JwtTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),

                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha512Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
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
