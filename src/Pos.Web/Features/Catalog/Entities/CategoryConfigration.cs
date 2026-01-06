using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Catalog.Entities
{
    public class CategoryConfigration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Path)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .IsRequired();

            // Find all decendents
            builder.HasIndex(c => c.Path);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.IconUrl)
                .HasMaxLength(255);

            builder.Property(c => c.Color)
                .HasMaxLength(9); // Matches hex codes (e.g., #RRGGBBAA)

            builder.Property(p => p.CreatedBy).HasMaxLength(36);
            builder.Property(p => p.ModifiedBy).HasMaxLength(36).IsRequired(false);

            // Self-referencing relationship
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
