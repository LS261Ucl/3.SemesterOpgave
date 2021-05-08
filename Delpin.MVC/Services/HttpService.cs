﻿using Delpin.Mvc.Helpers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delpin.MVC.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:5001/api/v1/";

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var response = await _httpClient.GetAsync(BaseUrl + url);

            if (!response.IsSuccessStatusCode)
                return new HttpResponseWrapper<T>(false, default, response);

            var responseDeserialized = await Deserialize<T>(response);

            return new HttpResponseWrapper<T>(true, responseDeserialized, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            string dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl + url, stringContent);

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