using MagicVilla_Utility;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public TokenDto GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                bool hasRefreshToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.RefreshToken, out string refreshToken);
                TokenDto token = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                    
                };
                return hasAccessToken ? token : null;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public void RemoveToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.RefreshToken);
        }

        public void SetToken(TokenDto tokenDto)
        {
            var cookieOptions = new CookieOptions
            {

                Expires = DateTime.UtcNow.AddDays(60)
            };
            _contextAccessor.HttpContext.Response.Cookies.Append(SD.AccessToken, tokenDto.AccessToken);
            _contextAccessor.HttpContext.Response.Cookies.Append(SD.RefreshToken, tokenDto.RefreshToken);
        }
    }
}
