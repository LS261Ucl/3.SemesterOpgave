using Delpin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delpin.Infrastructure.Data
{
    public class DelpinContext : DbContext
    {
        public DelpinContext(DbContextOptions<DelpinContext> options) : base(options)
        {
        }

        public DbSet<PostalCity> PostalCities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalLine> RentalLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(10, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}
