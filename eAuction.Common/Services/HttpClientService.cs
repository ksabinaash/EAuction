using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ILogger<HttpClientService> _logger;

        private readonly IHttpClientFactory _httpClient;

        public HttpClientService(ILogger<HttpClientService> logger, IHttpClientFactory httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
}

        public async Task<Product> ExecuteGet<T>(string url)
        {
            var client = _httpClient.CreateClient();

            var apiUrl = new Uri(url);

            HttpResponseMessage response = await client.GetAsync(apiUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());

                return new Product();
            }

            var result = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

            return result;            
        }

        public async Task<T> ExecutePost<T>(string url, T item)
        {
            var client = _httpClient.CreateClient();

            var apiUrl = new Uri(url);

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());

                return default(T);
            }

            var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

    }
}
