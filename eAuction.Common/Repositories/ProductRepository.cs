using eAuction.Common.Interfaces;
using eAuction.Common.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAuction.Common.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICosmosRepository _cosmosService;

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ILogger<ProductRepository> logger, ICosmosRepository cosmosService)
        {
            this._logger = logger;

            this._cosmosService = cosmosService;
        }

        public async Task<Product> AddProduct(Product product)
        {
            _logger.LogInformation("Began" + nameof(AddProduct));

            var response = await _cosmosService.AddItem<Product>(product, product.ProductId.ToString(), product.ProductName).ConfigureAwait(false);
  
            _logger.LogInformation("Ended" + nameof(AddProduct));

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return response.Resource;
            }

            else
            {
                return new Product();
            }
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _logger.LogInformation("Began" + nameof(DeleteProduct));

            var response = await _cosmosService.DeleteItem<Product>(product.ProductId.ToString(), product.ProductName).ConfigureAwait(false);

            var returnValue = (response.StatusCode != System.Net.HttpStatusCode.NoContent) ? false : true;

            _logger.LogInformation("Ended" + nameof(DeleteProduct));

            return returnValue;
        }

        public async Task<Product> GetProduct(int id)
        {
            _logger.LogInformation("Began" + nameof(GetProduct));

            var query = string.Format("select * from c where c.id ='{0}'", id.ToString());

            var response = await _cosmosService.QueryItems<Product>(query).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(GetProduct));

            return response.FirstOrDefault() ?? new Product();
        }

        public async Task<List<Product>> GetProducts()
        {
            _logger.LogInformation("Began" + nameof(GetProduct));

            var query = "select * from c";

            var response = await _cosmosService.QueryItems<Product>(query).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(GetProduct));

            return response ?? new List<Product>();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _logger.LogInformation("Began" + nameof(UpdateProduct));

            var response = await _cosmosService.UpdateItem<Product>(product, product.ProductId.ToString(), product.ProductName).ConfigureAwait(false);

            _logger.LogInformation("Ended" + nameof(UpdateProduct));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Resource;
            }

            else 
            {
                return new Product();
            }

        }
    }
}