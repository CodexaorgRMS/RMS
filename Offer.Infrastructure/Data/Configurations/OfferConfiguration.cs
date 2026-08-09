using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfferEntity = Offers.Domain.Entities.Offer;

namespace Offers.Infrastructure.Data.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<OfferEntity>
    {
        public void Configure(EntityTypeBuilder<OfferEntity> builder)
        {
            builder.HasKey(o => o.OfferId);

            builder.Property(o => o.OfferId)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.Description)
                .HasMaxLength(1000);

            builder.Property(o => o.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.Value)
                .IsRequired();

            builder.Property(o => o.StartDate)
                .IsRequired();

            builder.Property(o => o.EndDate)
                .IsRequired();

            builder.Property(o => o.IsSmart)
                .IsRequired();

            // Perfect Indexes
            builder.HasIndex(o => new { o.StartDate, o.EndDate })
                .HasDatabaseName("IX_Offer_StartDate_EndDate");
                
            builder.HasIndex(o => o.Type)
                .HasDatabaseName("IX_Offer_Type");
        }
    }
}
