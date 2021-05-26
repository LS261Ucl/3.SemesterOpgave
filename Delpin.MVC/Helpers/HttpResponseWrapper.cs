using System.Net.Http;

namespace Delpin.Mvc.Helpers
{
    // Generic wrapper for the HttpResponse that makes it easy to filter through the unnecessary data of the HttpResponse
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
    }
}