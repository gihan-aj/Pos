using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Catalog.Entities
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // -- Product --
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Sku).HasMaxLength(50).IsRequired(false);
            builder.HasIndex(p => p.Sku).IsUnique().HasFilter("[Sku] IS NOT NULL"); // Unique if present

            builder.Property(p => p.Description).HasMaxLength(250).IsRequired(false);
            builder.Property(p => p.Brand).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.Material).HasMaxLength(50).IsRequired(false);

            builder.Property(p => p.BasePrice).HasPrecision(18, 2);

            builder.Property(p => p.Tags).HasMaxLength(2000);

            builder.Property(p => p.CreatedBy).HasMaxLength(36);
            builder.Property(p => p.ModifiedBy).HasMaxLength(36).IsRequired(false);

            // Relationships
            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- VARIENTS ---
            //builder.OwnsMany(p => p.Variants, vb =>
            //{
            //    vb.ToTable("ProductVariants");
            //    vb.Property(v => v.Id).ValueGeneratedNever();
            //    vb.HasKey(p => p.Id);
            //    vb.WithOwner().HasForeignKey(v => v.ProductId);

            //    vb.Property(v => v.Sku).HasMaxLength(50).IsRequired();
            //    vb.HasIndex(v => v.Sku).IsUnique();

            //    vb.Property(v => v.Size).HasMaxLength(20);
            //    vb.Property(v => v.Color).HasMaxLength(30);

            //    vb.Property(v => v.Price).HasPrecision(18, 2);
            //    vb.Property(v => v.Cost).HasPrecision(18, 2);

            //    vb.Property(v => v.IsActive).IsRequired().HasDefaultValue(true);

            //    vb.Property(v => v.CreatedBy).HasMaxLength(36);
            //    vb.Property(v => v.ModifiedBy).HasMaxLength(36).IsRequired(false);
            //});

            // --- IMAGES ---
            builder.OwnsMany(p => p.Images, ib =>
            {
                ib.ToTable("ProductImages");
                ib.Property(i => i.Id).ValueGeneratedNever();
                ib.HasKey(i => i.Id);
                ib.WithOwner().HasForeignKey(i => i.ProductId);

                ib.Property(i => i.ImageUrl).HasMaxLength(250).IsRequired();
            });
        }
    }
}
