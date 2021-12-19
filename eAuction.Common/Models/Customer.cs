using System.ComponentModel.DataAnnotations;

namespace eAuction.Common.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Minimum 5 characters and Maximum 30 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Minimum 3 characters and Maximum 25 characters")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only alphabets allowed")]
        public string City { get; set; }

        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only alphabets allowed")]
        public string State { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers allowed")]
        public string Pin { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers allowed")]
        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Not a valid Email Format")]
        public string Email { get; set; }
    }
}