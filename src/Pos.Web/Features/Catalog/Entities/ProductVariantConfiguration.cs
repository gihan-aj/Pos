using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Catalog.Entities
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Sku)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(v => v.Sku).IsUnique();

            builder.Property(v => v.Size).HasMaxLength(20);
            builder.Property(v => v.Color).HasMaxLength(30);

            builder.Property(v => v.Price).HasPrecision(18, 2);
            builder.Property(v => v.Cost).HasPrecision(18, 2);

            builder.Property(v => v.StockQuantity).IsRequired();
            builder.Property(v => v.IsActive).HasDefaultValue(true);

            builder.Property(v => v.CreatedBy).HasMaxLength(36);
            builder.Property(v => v.ModifiedBy).HasMaxLength(36).IsRequired(false);

            builder.HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(v => v.StockQuantity)
                    .IsRowVersion(); // Timestamp for SQL Server
        }
    }
}
