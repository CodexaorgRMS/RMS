using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.Entities;
using OfferEntity = Offers.Domain.Entities.Offer;

namespace Offers.Infrastructure.Data.Configurations
{
    public class OfferTargetConfiguration : IEntityTypeConfiguration<OfferTarget>
    {
        public void Configure(EntityTypeBuilder<OfferTarget> builder)
        {
            builder.HasKey(ot => ot.OfferTargetId);

            builder.Property(ot => ot.OfferTargetId)
                .ValueGeneratedOnAdd();

            builder.Property(ot => ot.OfferId)
                .IsRequired();

            builder.Property(ot => ot.TargetType)
                .IsRequired();

            builder.Property(ot => ot.TargetId)
                .IsRequired();

            // Relationships
            builder.HasOne<OfferEntity>()
                .WithMany(o => o.Targets)
                .HasForeignKey(ot => ot.OfferId);

            // Indexes
            builder.HasIndex(ot => new { ot.OfferId, ot.TargetType, ot.TargetId })
                .IsUnique()
                .HasDatabaseName("IX_OfferTarget_UniqueTarget");
        }
    }
}
