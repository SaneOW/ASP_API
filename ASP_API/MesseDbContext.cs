using Microsoft.EntityFrameworkCore;
using ASP_API.Models;
using System;
using System.IO;
using System.Reflection;

namespace ASP_API
{
    public class MesseDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        public MesseDbContext(DbContextOptions<MesseDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguration der Many-to-Many Beziehung
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Customers)
                .UsingEntity(j => j.ToTable("CustomerProducts"));


            modelBuilder.Entity<Product>().HasData(
        new Product { ProductId = 1, ProductName = "Autos" },
        new Product { ProductId = 2, ProductName = "Smartphones" },
        new Product { ProductId = 3, ProductName = "Laptops" });
        }
    }
}

