using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eAuction.Common.Models
{
    public class Buyer : Customer
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage ="Bid Amount should be atleast greater that Rs. 1!")]
        public double BidAmount { get; set; }
    }

    public static class BuyerExtensions
    {
        public static List<Buyer> SortAmountByDescending(this List<Buyer> buyers)
        {
            if (buyers != null)
            {
                return buyers.OrderByDescending(b => b.BidAmount).ToList();
            }
            else 
            {
                return new List<Buyer>();
            }
        }
    }
}