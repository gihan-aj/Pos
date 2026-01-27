using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pos.Web.Features.Customers;

namespace Pos.Web.Features.Orders.Entities
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // -- Order Table --
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            // Financials
            builder.Property(o => o.SubTotal).HasPrecision(18, 2);
            builder.Property(o => o.DiscountAmount).HasPrecision(18, 2);
            builder.Property(o => o.TaxAmount).HasPrecision(18, 2);
            builder.Property(o => o.ShippingFee).HasPrecision(18, 2);
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Property(o => o.AmountPaid).HasPrecision(18, 2);
            builder.Property(o => o.AmountDue).HasPrecision(18, 2);

            // Address & Info
            builder.Property(o => o.DeliveryAddress).HasMaxLength(200).IsRequired();
            builder.Property(o => o.DeliveryCity).HasMaxLength(100).IsRequired(false);
            builder.Property(o => o.DeliveryRegion).HasMaxLength(100).IsRequired(false);
            builder.Property(o => o.DeliveryCountry).HasMaxLength(100).IsRequired(false);
            builder.Property(o => o.DeliveryPostalCode).HasMaxLength(20).IsRequired(false);
            builder.Property(o => o.TrackingNumber).HasMaxLength(100).IsRequired(false);
            builder.Property(o => o.Notes).HasMaxLength(200).IsRequired(false);

            builder.Property(o => o.PaymentMethod).HasMaxLength(50).IsRequired(false);

            builder.HasOne(o => o.Courier)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CourierId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(o => o.CreatedBy).HasMaxLength(36);
            builder.Property(o => o.ModifiedBy).HasMaxLength(36).IsRequired(false);

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- ORDER ITEMS (Owned Collection) ---
            builder.OwnsMany(o => o.OrderItems, ob =>
            {
                ob.ToTable("OrderItems");

                ob.HasKey(i => i.Id);
                ob.WithOwner().HasForeignKey(i => i.OrderId);

                // Snapshots
                ob.Property(i => i.ProductName).HasMaxLength(100).IsRequired();
                ob.Property(i => i.Sku).HasMaxLength(50).IsRequired();
                ob.Property(i => i.VariantDetails).HasMaxLength(200).IsRequired(false);

                // Money
                ob.Property(i => i.UnitPrice).HasPrecision(18, 2);
                ob.Property(i => i.UnitCost).HasPrecision(18, 2);
                ob.Property(i => i.DiscountAmount).HasPrecision(18, 2);
                ob.Property(i => i.SubTotal).HasPrecision(18, 2);

                // Audit fields for items
                ob.Property(i => i.CreatedBy).HasMaxLength(36);
                ob.Property(i => i.ModifiedBy).HasMaxLength(36).IsRequired(false);
            });

            // Navigation Metadata
            builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
