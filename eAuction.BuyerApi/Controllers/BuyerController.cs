using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eAuction.BuyerApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly ILogger<BuyerController> _logger;

        private readonly IBuyerService _service;

        public BuyerController(ILogger<BuyerController> logger, IBuyerService service)
        {
            this._logger = logger;

            this._service = service;
        }

        [HttpPost("place-bid")]
        [Produces(typeof(Buyer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Buyer>> AddBid(Buyer bid)
        {
            _logger.LogInformation("Began" + nameof(AddBid));

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.AddBid(bid).ConfigureAwait(false);

                return Ok(response);
            }

            catch (Exception ex)
            {
                if (ex is ProductException)
                {
                    return NotFound(ex.Message);
                }

                else if (ex is BuyerException || ex is ValidationException)
                {
                    return BadRequest(ex.Message);
                }

                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(AddBid));
            }
        }

        [HttpPut("update-bid/{productId}/{buyerEmailld}/{newBidAmount}")]
        [Produces(typeof(Buyer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Buyer>> UpdateBid(int productId, string buyerEmailld, double newBidAmount)
        {
            _logger.LogInformation("Began" + nameof(UpdateBid));

            try
            {
                var response = await _service.UpdateBid(productId, buyerEmailld, newBidAmount).ConfigureAwait(false);

                return Ok(response);
            }

            catch (Exception ex)
            {
                if (ex is ProductException || ex is BuyerException)
                {
                    return NotFound(ex.Message);
                }

                else if (ex is ValidationException)
                {
                    return BadRequest(ex.Message);
                }

                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            finally
            {
                _logger.LogInformation("Ended" + nameof(UpdateBid));
            }
        }

        
    }
}
