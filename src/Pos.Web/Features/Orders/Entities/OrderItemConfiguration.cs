using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Orders.Entities
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(i => i.Id);

            // Explicitly map the Foreign Key since we are not "Owning" it anymore
            builder.Property(i => i.OrderId).IsRequired();

            builder.HasOne(i => i.ProductVariant)
                .WithMany()
                .HasForeignKey(i => i.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Snapshots
            builder.Property(i => i.ProductName).HasMaxLength(100).IsRequired();
            builder.Property(i => i.Sku).HasMaxLength(50).IsRequired();
            builder.Property(i => i.VariantDetails).HasMaxLength(200).IsRequired(false);

            // Money
            builder.Property(i => i.UnitPrice).HasPrecision(18, 2);
            builder.Property(i => i.UnitCost).HasPrecision(18, 2);
            builder.Property(i => i.DiscountAmount).HasPrecision(18, 2);
            builder.Property(i => i.SubTotal).HasPrecision(18, 2);

            // Audit
            builder.Property(i => i.CreatedBy).HasMaxLength(36);
            builder.Property(i => i.ModifiedBy).HasMaxLength(36).IsRequired(false);
        }
    }
}
