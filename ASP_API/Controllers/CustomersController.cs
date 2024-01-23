using ASP_API.Models;
using ASP_API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_API.Dtos;


namespace Project_Messe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly MesseDbContext _context;

        public CustomersController(MesseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customerDtos = await _context.Customers
                .Include(c => c.Products) // Include products to access ProductIds
                .Select(c => new CustomerDto
                {
                    LastName = c.LastName,
                    FirstName = c.FirstName,
                    StreetAddress = c.StreetAddress,
                    HouseNumber = c.HouseNumber,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Picture = c.Picture,
                    ProductIds = c.Products.Select(p => p.ProductId).ToList() // Convert products to ProductIds
                })
                .ToListAsync();

            return customerDtos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customerDto = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Include(c => c.Products) // Include products to access ProductIds
                .Select(c => new CustomerDto
                {
                    LastName = c.LastName,
                    FirstName = c.FirstName,
                    StreetAddress = c.StreetAddress,
                    HouseNumber = c.HouseNumber,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Picture = c.Picture,
                    ProductIds = c.Products.Select(p => p.ProductId).ToList() // Convert products to ProductIds
                })
                .FirstOrDefaultAsync();

            if (customerDto == null)
            {
                return NotFound();
            }

            return customerDto;
        }
        //  /api/customer/search?name=John
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Der Suchname darf nicht leer sein.");
            }

            var customerDtos = await _context.Customers
                .Include(c => c.Products) // Include products to access ProductIds
                .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
                .Select(c => new CustomerDto
                {
                    LastName = c.LastName,
                    FirstName = c.FirstName,
                    StreetAddress = c.StreetAddress,
                    HouseNumber = c.HouseNumber,
                    City = c.City,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Picture = c.Picture,
                    ProductIds = c.Products.Select(p => p.ProductId).ToList() // Convert products to ProductIds
                })
                .ToListAsync();

            return customerDtos;
        }


        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDto customerDto)
        {


            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.LastName = customerDto.LastName;
            // ... Update other properties ...

            // Update relationships
            customer.Products = await _context.Products
                .Where(p => customerDto.ProductIds.Contains(p.ProductId))
                .ToListAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            var customer = new Customer
            {
                LastName = customerDto.LastName,
                FirstName = customerDto.FirstName,
                StreetAddress = customerDto.StreetAddress,
                HouseNumber = customerDto.HouseNumber,
                City = customerDto.City,
                PostalCode = customerDto.PostalCode,
                Country = customerDto.Country,
                Picture = customerDto.Picture,
                Products = new List<Product>()
            };

            foreach (var productId in customerDto.ProductIds)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    customer.Products.Add(product);
                }
                else
                {
                    // Wenn das Produkt nicht gefunden wird, können Sie entscheiden, wie Sie vorgehen möchten.
                    return NotFound($"Product with ID {productId} not found.");
                }
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
