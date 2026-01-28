using Microsoft.EntityFrameworkCore;
using Pos.Web.Features.Catalog.Entities;
using Pos.Web.Features.Couriers.Entities;
using Pos.Web.Features.Customers;
using Pos.Web.Features.Orders.Entities;

namespace Pos.Web.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<OrderPayment> OrderPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Scan assembly for Entity Configurations (IEntityTypeConfiguration)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
