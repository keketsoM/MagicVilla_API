using System.Net;

namespace MagicVilla_Web.Model
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public Object Result { get; set; }
    }
}
