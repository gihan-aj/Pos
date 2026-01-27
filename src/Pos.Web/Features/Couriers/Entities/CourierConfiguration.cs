using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Couriers.Entities
{
    public class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.ToTable("Couriers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(c => c.Name)
                .IsUnique(); // Prevent duplicate courier names

            builder.Property(c => c.TrackingUrlTemplate)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.WebsiteUrl)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(50)
                .IsRequired(false);

            // Audit fields
            builder.Property(c => c.CreatedBy).HasMaxLength(36);
            builder.Property(c => c.ModifiedBy).HasMaxLength(36).IsRequired(false);
        }
    }
}
