using MagicVilla_Web.Model.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(TokenDto token);  
        TokenDto? GetToken();
        void RemoveToken(); 
    }
}
