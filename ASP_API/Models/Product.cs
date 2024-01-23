using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP_API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        // Many-to-Many relationship with Customer
        public List<Customer> Customers { get; set; }
    }
}
