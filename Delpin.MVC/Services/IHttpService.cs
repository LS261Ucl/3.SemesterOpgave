using Delpin.Mvc.Helpers;
using System.Threading.Tasks;

namespace Delpin.MVC.Services
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url, string token = null);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data, string token = null);
    }
}