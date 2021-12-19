using System.ComponentModel.DataAnnotations;

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
}