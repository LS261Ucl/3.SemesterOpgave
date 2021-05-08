using Delpin.Mvc.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delpin.MVC.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("BaseApiUrl"));
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new HttpResponseWrapper<T>(false, default, response);

            var responseDeserialized = await Deserialize<T>(response);

            return new HttpResponseWrapper<T>(true, responseDeserialized, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            string dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                return new HttpResponseWrapper<TResponse>(false, default, response);

            var responseDeserialized = await Deserialize<TResponse>(response);
            return new HttpResponseWrapper<TResponse>(true, responseDeserialized, response);
        }

        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse)
        {
            var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string response = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, serializerOptions);
        }
    }
}
