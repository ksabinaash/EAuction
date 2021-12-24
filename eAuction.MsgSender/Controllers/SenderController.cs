﻿using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eAuction.MsgSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenderController : ControllerBase
    {
        private readonly ILogger<SenderController> _logger;

        private readonly IAzureMsgSenderService _service;

        private readonly IConfiguration _config;

        public SenderController(ILogger<SenderController> logger, IAzureMsgSenderService service, IConfiguration config)
        {
            this._logger = logger;

            this._service = service;

            this._config = config;
        }

        [HttpPost("addbid-queuemsg")]
        [Produces(typeof(bool))]        
        public virtual async Task<ActionResult<bool>> AddBidQueueMsg(Buyer bid)
        {
            _logger.LogInformation("Began" + nameof(AddBidQueueMsg));

            try
            {
                var bidJsonMsg = JsonConvert.SerializeObject(bid);

                var queueName = _config.GetSection("AzureSBBidQueue").Value;

                await _service.SendMessage(bidJsonMsg, queueName).ConfigureAwait(false);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(AddBidQueueMsg));
            }
        }

        [HttpPost("addproductid-queuemsg")]
        [Produces(typeof(bool))]
        public virtual async Task<ActionResult<bool>> AddProductIdMsg(int productId)
        {
            _logger.LogInformation("Began" + nameof(AddProductIdMsg));

            try
            {
                var productJsonMsg = JsonConvert.SerializeObject(productId);

                var queueName = _config.GetSection("AzureSBProductQueue").Value;

                await _service.SendMessage(productJsonMsg, queueName).ConfigureAwait(false);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(AddProductIdMsg));
            }
        }
    }
}