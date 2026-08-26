using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.Entities;

namespace Offers.Infrastructure.Data.Configurations;

public class OfferTargetConfiguration : IEntityTypeConfiguration<OfferTarget>
{
    public void Configure(EntityTypeBuilder<OfferTarget> builder)
    {
        builder.ToTable("OfferTargets");

        builder.HasKey(ot => ot.OfferTargetId);

        builder.Property(ot => ot.OfferId)
            .IsRequired();

        builder.Property(ot => ot.TargetType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(ot => ot.TargetId)
            .IsRequired();

        builder.Property(ot => ot.RequiredQuantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(ot => ot.SpecialPrice)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(ot => new { ot.OfferId, ot.TargetType, ot.TargetId })
            .HasDatabaseName("IX_OfferTarget_Offer_Type_Target");

        builder.HasIndex(ot => ot.TargetId)
            .HasDatabaseName("IX_OfferTarget_TargetId");
    }
}
