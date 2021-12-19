using eAuction.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();

        Task<Product> GetProduct(int Id);

        Task<Product> AddProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task<bool> DeleteProduct(Product product);
    }
}
