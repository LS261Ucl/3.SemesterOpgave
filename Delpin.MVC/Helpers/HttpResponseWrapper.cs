using System.Net.Http;
using System.Threading.Tasks;

namespace Delpin.Mvc.Helpers
{
    public class HttpResponseWrapper<T>
    {
        public bool Success { get; set; }
        public T Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public HttpResponseWrapper(bool success, T response, HttpResponseMessage httpResponseMessage)
        {
            Success = success;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public async Task<string> GetResponseBody()
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}