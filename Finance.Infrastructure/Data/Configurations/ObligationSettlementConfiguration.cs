using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Data.Configurations;

public sealed class ObligationSettlementConfiguration : IEntityTypeConfiguration<ObligationSettlement>
{
    public void Configure(EntityTypeBuilder<ObligationSettlement> builder)
    {
        builder.ToTable("ObligationSettlements");

        builder.HasKey(x => x.SettlementId);

        builder.Property(x => x.AmountPaid)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Source)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.HasIndex(x => x.ObligationId);
        builder.HasIndex(x => x.ShiftId);
        builder.HasIndex(x => x.SettledAt);
    }
}
