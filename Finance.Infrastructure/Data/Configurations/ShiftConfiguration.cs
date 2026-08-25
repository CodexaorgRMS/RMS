using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Data.Configurations;

public sealed class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("Shifts");

        builder.HasKey(x => x.ShiftId);

        builder.Property(x => x.OpeningFloat)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ActualCashEnd)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExpectedCashEnd)
            .HasPrecision(18, 2);

        builder.Property(x => x.Variance)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.HasIndex(x => new { x.CashierId, x.Status });
    }
}
