namespace ASP_API.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        // Contains IDs of related customers
        public List<int> CustomerIds { get; set; }
    }
}
