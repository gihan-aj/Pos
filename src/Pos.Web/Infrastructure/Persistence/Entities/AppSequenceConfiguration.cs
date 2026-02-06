using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pos.Web.Infrastructure.Persistence.Entities;

public class AppSequenceConfiguration : IEntityTypeConfiguration<AppSequence>
{
    public void Configure(EntityTypeBuilder<AppSequence> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(50);

        builder.Property(x => x.Prefix)
            .HasMaxLength(10);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasData(
            new AppSequence { Id = "Order", Prefix = "ORD-", CurrentValue = 1000, Increment = 1 },
            new AppSequence { Id = "Sku", Prefix = "PROD-", CurrentValue = 10000, Increment = 1 }
        );
    }
}
