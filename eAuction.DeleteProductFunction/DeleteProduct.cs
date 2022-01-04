using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eAuction.DeleteProductFunction
{
    public static class DeleteProduct
    {
        [FunctionName("DeleteProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string productId = req.Query["productId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            productId = productId ?? data?.productId;

            if (string.IsNullOrWhiteSpace(productId))
            {
                return new BadRequestObjectResult("Product Id is missing!");
            }

            string url = "https://eauctionsbclient.azurewebsites.net/api/MessageSR/add-productidmsg?productId=";

            string msgQueueUrl = string.Format("{0}{1}", url, productId);

            var result = await ExecutePost(msgQueueUrl, productId, log);

            string status = (result == true) ? "Successful." : "Not Successful";
        
            log.LogInformation("Adding message in queue is "+ status);

            return new OkObjectResult(result);
        }

        public static async Task<bool> ExecutePost(string url, object item, ILogger log)
        {
            HttpClient _httpClient = new HttpClient();

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, httpContent).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                log.LogError(response.StatusCode.ToString());

                return false;
            }

            return true;
        }
    }
}
