using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    public class SellerService : ISellerService
    {
        private readonly ILogger<SellerService> _logger;

        private readonly IProductRepository _repository;

        public SellerService(ILogger<SellerService> logger, IProductRepository repository)
        {
            this._logger = logger;

            this._repository = repository;
        }
        public async Task<Product> AddProduct(Product product)
        {
            _logger.LogInformation("Began" + nameof(AddProduct));

            await this.GetProduct(Convert.ToInt32(product.ProductId), false).ConfigureAwait(false);

            var response = await _repository.AddProduct(product).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(AddProduct));

            return response ?? new Product();
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            _logger.LogInformation("Began" + nameof(DeleteProduct));

            var product = await this.GetProduct(productId, true).ConfigureAwait(false);

            var response = await _repository.DeleteProduct(product).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(DeleteProduct));

            return response;
        }

        public async Task<Product> GetProductBids(int productId)
        {
            _logger.LogInformation("Began" + nameof(GetProductBids));

            var product = await this.GetProduct(productId, true).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(GetProductBids));

            return product;
        }

        private async Task<Product> GetProduct(int id, bool isProductExpected)
        {
            var product = new Product();

            product = await _repository.GetProduct(id).ConfigureAwait(false);

            product.Buyers = product?.Buyers?.OrderByDescending(b => b.BidAmount).ToList();

            if (isProductExpected && product.ProductId == null)
            {
                throw new ProductException("Product Id not available!");
            }

            if (!isProductExpected && product.ProductId != null)
            {
                throw new ProductException("Product Id already available!");
            }

            return product;
        }

        public void VaidateDeleteProduct(Product product)
        {
            if (product.Buyers?.Count > 0)
            {
                throw new ValidationException("Product can't be deleted, since it has atleast one bid!");
            }

            if (product.BidEndDate <= DateTime.Now)
            {
                throw new ValidationException("roduct can't be deleted, since bid end date has completed!");
            }
        }
    }
}