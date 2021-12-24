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

        private readonly IAzureMsgReceiverService _service;

        private readonly IConfiguration _config;

        private readonly IHttpClientService _httpClientService;

        public ReceiverController(ILogger<ReceiverController> logger, IAzureMsgReceiverService service, IConfiguration config, IHttpClientService httpClientService)
        {
            this._logger = logger;

            this._service = service;

            this._config = config;

            this._httpClientService = httpClientService;
        }

        [HttpPost("execute-addbid")]
        public virtual async Task<ActionResult<bool>> ReadAndExecuteAddBidMsg()
        {
            _logger.LogInformation("Began" + nameof(ReadAndExecuteAddBidMsg));

            try
            {
                var queueName = _config.GetSection("AzureSBBidQueue").Value;

                var bidJsonMsg = await _service.ReceiveMessage(queueName).ConfigureAwait(false);

                var addBidUrl = _config.GetSection("AddBidUrl").Value;

                var buyer = JsonConvert.DeserializeObject<Buyer>(bidJsonMsg);

                await _httpClientService.ExecutePost<Buyer>(addBidUrl, buyer);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(ReadAndExecuteAddBidMsg));
            }
        }

        [HttpPost("execute-getbids")]
        [Produces(typeof(bool))]
        public virtual async Task<ActionResult<bool>> ReadAndExecuteProductIdMsg()
        {
            _logger.LogInformation("Began" + nameof(ReadAndExecuteProductIdMsg));

            try
            {
                var queueName = _config.GetSection("AzureSBProductQueue").Value;

                var productJsonMsg = await _service.ReceiveMessage(queueName).ConfigureAwait(false);

                var apiUrl = _config.GetSection("GetProductUrl").Value;

                var productId = JsonConvert.DeserializeObject<int>(productJsonMsg);

                var getProductUrl = string.Format("{0}/{1}", apiUrl, productId);

                await _httpClientService.ExecuteGet<Product>(getProductUrl);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(ReadAndExecuteProductIdMsg));
            }
        }
    }
}