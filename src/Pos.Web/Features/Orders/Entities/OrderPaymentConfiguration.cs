using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Features.Orders.Entities
{
    public class OrderPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
    {
        public void Configure(EntityTypeBuilder<OrderPayment> builder)
        {
            builder.ToTable("OrderPayments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .IsRequired();

            builder.Property(p => p.TransactionId)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.Notes)
                .HasMaxLength(200)
                .IsRequired(false);

            // Audit
            builder.Property(p => p.CreatedBy).HasMaxLength(36);
            builder.Property(p => p.ModifiedBy).HasMaxLength(36).IsRequired(false);
        }
    }
}
