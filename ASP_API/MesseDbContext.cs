using ASP_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ASP_API
{
    public class MesseDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer_Product> Customer_Products { get; set; }


        public MesseDbContext(DbContextOptions<MesseDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer_Product>()
                .HasKey(cp => new { cp.CustomerId, cp.ProductId }); // Composite key

            modelBuilder.Entity<Customer_Product>()
                .HasOne(cp => cp.Customer)
                .WithMany(c => c.CustomerProducts)
                .HasForeignKey(cp => cp.CustomerId);

            modelBuilder.Entity<Customer_Product>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CustomerProducts)
                .HasForeignKey(cp => cp.ProductId);

            modelBuilder.Entity<Product>().HasData(
    new Product { ProductId = 1, ProductName = "Autos" },
    new Product { ProductId = 2, ProductName = "Smartphones" },
    new Product { ProductId = 3, ProductName = "Laptops" });
        }
    }
}

