using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.Entities;

namespace Offers.Infrastructure.Data.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");

        builder.HasKey(o => o.OfferId);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(o => o.Description)
            .HasMaxLength(1000);

        builder.Property(o => o.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(o => o.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.StartDate)
            .IsRequired();

        builder.Property(o => o.EndDate)
            .IsRequired();

        builder.Property(o => o.IsActive)
            .IsRequired();

        builder.Property(o => o.IsSmart)
            .IsRequired();

        builder.Property(o => o.Priority)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.HasMany(o => o.Targets)
            .WithOne()
            .HasForeignKey(t => t.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(o => new { o.IsActive, o.StartDate, o.EndDate })
            .HasDatabaseName("IX_Offer_Active_Dates");

        builder.HasIndex(o => o.Type)
            .HasDatabaseName("IX_Offer_Type");

        builder.HasIndex(o => o.Priority)
            .HasDatabaseName("IX_Offer_Priority");
    }
}
