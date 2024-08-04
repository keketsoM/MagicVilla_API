using MagicVilla_Web.Model;

namespace MagicVilla_Web.Services.IServices
{
    public interface IApiMessageRequestBuilder
    {
        HttpRequestMessage Build(APIRequest apiRequest);
    }
}
