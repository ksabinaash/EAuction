using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eAuction.Common.Models
{
    public class Product
    {
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers allowed")]
        [JsonProperty(PropertyName = "id")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Minimum 5 characters and Maximum 30 characters")]
        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        [Required(ErrorMessage = "Required")]
        [EnumDataType(typeof(ProductCategoryEnum), ErrorMessage = "Available Categories: Paiting, Sculptor, Ornament")]
        public ProductCategoryEnum Category { get; set; }

        [Required(ErrorMessage = "Required")]
        public double StartingPrice { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime BidEndDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public Seller Seller { get; set; }

        public List<Buyer> Buyers { get; set; }
    }

    public enum ProductCategoryEnum
    {
        Painting,
        Sculptor,
        Ornament
    }
}
