using eAuction.Common.Models;
using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IBuyerService
    {
        Task<Buyer> AddBid(Buyer bid);

        Task<Buyer> UpdateBid(int productId, string email, double amount);
    }
}