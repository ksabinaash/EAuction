using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eAuction.MsgReceiver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiverController : ControllerBase
    {
        private readonly ILogger<ReceiverController> _logger;

        private readonly IAzureMsgService _service;

        private readonly IConfiguration _config;

        private readonly IHttpClientService _httpClientService;

        public ReceiverController(ILogger<ReceiverController> logger, IAzureMsgService service, IConfiguration config, IHttpClientService httpClientService)
        {
            this._logger = logger;

            this._service = service;

            this._config = config;

            this._httpClientService = httpClientService;
        }

        [HttpGet("execute-addbid")]
        public virtual async Task<ActionResult<Buyer>> ReadAndExecuteAddBidMsg()
        {
            _logger.LogInformation("Began" + nameof(ReadAndExecuteAddBidMsg));

            try
            {
                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBBidQueueName").Value;

                var bidJsonMsg = await _service.ReceiveMessage(sbConnString, queueName).ConfigureAwait(false);

                var addBidUrl = _config.GetSection("AddBidUrl").Value;

                var buyer = JsonConvert.DeserializeObject<Buyer>(bidJsonMsg);

                var buyerResult = await _httpClientService.ExecutePost<Buyer>(addBidUrl, buyer);

                return buyerResult ?? new Buyer();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new Buyer();
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(ReadAndExecuteAddBidMsg));
            }
        }

        [HttpGet("execute-getbids")]
        [Produces(typeof(bool))]
        public virtual async Task<ActionResult<List<Buyer>>> ReadAndExecuteProductIdMsg()
        {
            _logger.LogInformation("Began" + nameof(ReadAndExecuteProductIdMsg));

            try
            {
                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBProductQueueName").Value;

                var productJsonMsg = await _service.ReceiveMessage(sbConnString, queueName).ConfigureAwait(false);

                var apiUrl = _config.GetSection("GetProductUrl").Value;

                var productId = JsonConvert.DeserializeObject<int>(productJsonMsg);

                var getProductUrl = string.Format("{0}/{1}", apiUrl, productId);

                var product = await _httpClientService.ExecuteGet<Product>(getProductUrl);

                return product?.Buyers ?? new List<Buyer>();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new List<Buyer>();
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(ReadAndExecuteProductIdMsg));
            }
        }
    }
}