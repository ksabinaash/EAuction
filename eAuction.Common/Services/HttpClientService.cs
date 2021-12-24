using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class HttpClientService
    {
        private readonly ILogger<HttpClientService> _logger;

        private readonly IHttpClientFactory _httpClient;

        public HttpClientService(ILogger<HttpClientService> logger, IHttpClientFactory httpClient)
        {
            this._logger = logger;

            this._httpClient = httpClient;
}

        public async Task<T> ExecuteGet<T>(string url)
        {
            var httpClient = _httpClient.CreateClient();

            var apiUrl = new Uri(url);

            using (var response = await httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                var responseMsg = response.EnsureSuccessStatusCode();

                if (responseMsg.IsSuccessStatusCode)
                {
                    _logger.LogInformation(nameof(ExecuteGet) + "is success");
                }
                else
                {
                    HttpRequestException exception = new HttpRequestException(string.Format(
                      CultureInfo.InvariantCulture,
                      "Pinging search engine {0}. Response status code does not indicate success: {1} ({2}).",
                      url,
                      (int)response.StatusCode,
                      response.ReasonPhrase));

                    Exception ex = exception as Exception;

                    _logger.LogError(ex, ex.Message);
                }

                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                var result = JsonConvert.DeserializeObject<T>(stream.ToString());

                _logger.LogInformation("Ended " + nameof(ExecuteGet));

                return result;
            }
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
            }

            var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

    }
}
