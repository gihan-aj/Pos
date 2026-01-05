using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Catalog.Entities
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // -- Product --
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Sku).HasMaxLength(50).IsRequired(false);
            builder.HasIndex(p => p.Sku).IsUnique().HasFilter("[Sku] IS NOT NULL"); // Unique if present

            builder.Property(p => p.BasePrice).HasPrecision(18, 2);

            // Comparer logic
            var tagComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2), // how to compare
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // how to hash
                c => c.ToList()); // How to snapshot (deep copy)

            // Tags: Store as JSON string in SQL Server
            builder.Property(p => p.Tags)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                )
                .Metadata.SetValueComparer(tagComparer);

            builder.Property(p => p.Tags).HasMaxLength(2000);

            // Relationships
            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- VARIENTS ---
            builder.OwnsMany(p => p.Varients, vb =>
            {
                vb.ToTable("ProductVarients");
                vb.Property(v => v.Id).ValueGeneratedNever();
                vb.HasKey(p => p.Id);
                vb.WithOwner().HasForeignKey(v => v.ProductId);

                vb.Property(v => v.Sku).HasMaxLength(50).IsRequired();
                vb.HasIndex(v => v.Sku).IsUnique();

                vb.Property(v => v.Size).HasMaxLength(20);
                vb.Property(v => v.Color).HasMaxLength(30);

                vb.Property(v => v.Price).HasPrecision(18, 2);
                vb.Property(v => v.Cost).HasPrecision(18, 2);
            });

            // --- IMAGES ---
            builder.OwnsMany(p => p.Images, ib =>
            {
                ib.ToTable("ProductImages");
                ib.Property(i => i.Id).ValueGeneratedNever();
                ib.HasKey(i => i.Id);
                ib.WithOwner().HasForeignKey(i => i.ProductId);

                ib.Property(i => i.ImageUrl).HasMaxLength(500).IsRequired();
            });
        }
    }
}
