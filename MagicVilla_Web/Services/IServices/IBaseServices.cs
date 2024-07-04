using MagicVilla_Web.Model;


namespace MagicVilla_Web.Services.IServices
{
    public interface IBaseServices
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true);
    }
}
