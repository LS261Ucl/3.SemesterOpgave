using System.Threading.Tasks;
using Delpin.Mvc.Helpers;

namespace Delpin.MVC.Services
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data);
    }
}