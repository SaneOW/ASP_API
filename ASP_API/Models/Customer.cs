using System.ComponentModel.DataAnnotations;

namespace ASP_API.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(200)]
        public string StreetAddress { get; set; }

        [StringLength(10)]
        public string HouseNumber { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        public byte[] Picture { get; set; }

        public ICollection<Customer_Product> CustomerProducts { get; set; } = new List<Customer_Product>();
    }
}
