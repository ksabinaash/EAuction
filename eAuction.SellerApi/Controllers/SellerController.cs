using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eAuction.SellerApi.Controllers
{
    [Route("e-auction/api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ILogger<SellerController> _logger;

        private readonly ISellerService _service;

        public SellerController(ILogger<SellerController> logger, ISellerService service)
        {
            this._logger = logger;

            this._service = service;
        }

        [HttpGet("{id}")]
        [Produces(typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInformation("Began" + nameof(GetProduct));

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.GetProductBids(id).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is ProductException)
                {
                    return NotFound(ex.Message);
                }

                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            finally
            {
                _logger.LogInformation("Ended" + nameof(GetProduct));
            }
        }

        [HttpPost("add-product")]
        [Produces(typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            _logger.LogInformation("Began" + nameof(AddProduct));

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.AddProduct(product).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is ProductException)
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
                _logger.LogInformation("Ended" + nameof(AddProduct));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            _logger.LogInformation("Began" + nameof(DeleteProduct));

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.DeleteProduct(id).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is ProductException || ex is ValidationException)
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
                _logger.LogInformation("Ended" + nameof(DeleteProduct));
            }   
        }
    }
}