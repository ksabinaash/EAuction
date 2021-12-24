using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            this.ValidateAddProduct(product);

            var response = await _repository.AddProduct(product).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(AddProduct));

            return response ?? new Product();
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            _logger.LogInformation("Began" + nameof(DeleteProduct));

            var product = await this.GetProduct(productId, true).ConfigureAwait(false);

            this.VaidateDeleteProduct(product);

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

        public async Task<List<Product>> GetProducts()
        {
            _logger.LogInformation("Began" + nameof(GetProducts));

            var products = await _repository.GetProducts().ConfigureAwait(false);

            products?.ForEach(p =>
            {
                p.Buyers = p.Buyers.SortAmountByDescending();
            });

            _logger.LogInformation("Ended" + nameof(GetProducts));

            return products;
        }

        private async Task<Product> GetProduct(int id, bool isProductExpected)
        {
            var product = new Product();

            product = await _repository.GetProduct(id).ConfigureAwait(false);            

            if (isProductExpected && product.ProductId == null)
            {
                throw new ProductException("Product Id not available!");
            }

            if (!isProductExpected && product.ProductId != null)
            {
                throw new ProductException("Product Id already available!");
            }

            product.Buyers = product?.Buyers.SortAmountByDescending();

            return product;
        }

        private void ValidateAddProduct(Product product)
        {
            if (product.Buyers != null && product.Buyers?.Count > 0)
            {
                throw new ValidationException("Bids can't be added while creating a new Product!");
            }
        }

        private void VaidateDeleteProduct(Product product)
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