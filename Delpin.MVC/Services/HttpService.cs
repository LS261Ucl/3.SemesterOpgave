using Delpin.Mvc.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Delpin.MVC.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Gets the API address from appsettings.json so it can be overridden during deployment
            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("BaseApiUrl"));
        }

        // Generic Get method that fetches from the API with the Url through the HttpClient
        // Afterwards it will use the parameterized class to deserialize the response and return a HttpResponseWrapper
        public async Task<HttpResponseWrapper<T>> Get<T>(string url, string token = null)
        {
            SetRequestHeader(token);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new HttpResponseWrapper<T>(false, default, response);

            var responseDeserialized = await Deserialize<T>(response);

            return new HttpResponseWrapper<T>(true, responseDeserialized, response);
        }

        // Generic Create method that expects an input class and expected output class
        // Sends Post request to API with the serialized class and returns the HttpResponseWrapper
        public async Task<HttpResponseWrapper<TResponse>> Create<T, TResponse>(string url, T data, string token = null)
        {
            SetRequestHeader(token);

            string dataJson = JsonSerializer.Serialize(data, IgnoreNullSerializerOption());
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponseWrapper<TResponse>(false, default, response);
            }


            var responseDeserialized = await Deserialize<TResponse>(response);
            return new HttpResponseWrapper<TResponse>(true, responseDeserialized, response);
        }

        // Generic Update method that expects an input class and sends a Put request to the Api
        public async Task<HttpResponseWrapper<object>> Update<T>(string url, T data, string token = null)
        {
            SetRequestHeader(token);

            var dataJson = JsonSerializer.Serialize(data, IgnoreNullSerializerOption());
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponseWrapper<object>(false, await response.Content.ReadAsStringAsync(), response);
            }

            return new HttpResponseWrapper<object>(true, response.IsSuccessStatusCode, response);
        }

        // Sends a Delete request to the APi with the given url. 
        public async Task<HttpResponseWrapper<object>> Delete(string url, string token = null)
        {
            SetRequestHeader(token);

            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
                return new HttpResponseWrapper<object>(false, default, response);

            return new HttpResponseWrapper<object>(true, response.IsSuccessStatusCode, response);
        }

        // Deserialize HttpResponse from json to C# objects 
        private static async Task<T> Deserialize<T>(HttpResponseMessage httpResponse)
        {
            var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve };
            string response = await httpResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(response, serializerOptions);
        }

        private static JsonSerializerOptions IgnoreNullSerializerOption()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            return serializerOptions;
        }

        // Takes a token and sets it as AuthenticationHeader to make an authenticated request to the API
        private void SetRequestHeader(string token)
        {
            if (token == null)
                return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
