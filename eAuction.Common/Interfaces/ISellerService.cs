using eAuction.Common.Models;
using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface ISellerService
    {
        Task<Product> AddProduct(Product product);

        Task<bool> DeleteProduct(int productId);

        Task<Product> GetProductBids(int productId);
    }
}