namespace ASP_API.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        // Navigation property for the join table
        public ICollection<Customer_Product> CustomerProducts { get; set; }
    }
}
