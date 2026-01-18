using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Customers
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.HasIndex(c => c.PhoneNumber)
                .IsUnique();

            builder.Property(c => c.Email)
                .HasMaxLength(100);
            builder.Property(c => c.Address)
                .HasMaxLength(200);
            builder.Property(c => c.City)
                .HasMaxLength(50);
            builder.Property(c => c.Country)
                .HasMaxLength(50);
            builder.Property(c => c.PostalCode)
                .HasMaxLength(20);
            builder.Property(c => c.Region)
                .HasMaxLength(50);
            builder.Property(c => c.Notes)
                .HasMaxLength(500);

            builder.Property(p => p.CreatedBy).HasMaxLength(36);
            builder.Property(p => p.ModifiedBy).HasMaxLength(36).IsRequired(false);
        }
    }   
}
