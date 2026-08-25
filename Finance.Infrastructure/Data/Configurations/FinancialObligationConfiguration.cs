using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Data.Configurations;

public sealed class FinancialObligationConfiguration : IEntityTypeConfiguration<FinancialObligation>
{
    public void Configure(EntityTypeBuilder<FinancialObligation> builder)
    {
        builder.ToTable("FinancialObligations");

        builder.HasKey(x => x.ObligationId);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PaidAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.RemainingAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.HasMany(x => x.Settlements)
            .WithOne(x => x.Obligation)
            .HasForeignKey(x => x.ObligationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.DueDate);
    }
}
