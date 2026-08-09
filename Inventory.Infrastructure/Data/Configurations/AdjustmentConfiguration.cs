using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class AdjustmentConfiguration : IEntityTypeConfiguration<Adjustment>
    {
        public void Configure(EntityTypeBuilder<Adjustment> builder)
        {
            builder.HasKey(a => a.AdjustmentId);

            builder.Property(a => a.AdjustmentId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.ProductId)
                .IsRequired();

            builder.Property(a => a.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Quantity)
                .IsRequired();

            builder.Property(a => a.Note)
                .HasMaxLength(500);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.CreatedBy)
                .IsRequired();

            builder.HasOne(a => a.Product)
                .WithMany(p => p.Adjustments)
                .HasForeignKey(a => a.ProductId);

            // Cashier mapping removed because the entity does not exist

            // Perfect Indexes
            builder.HasIndex(a => new { a.ProductId, a.CreatedAt })
                .HasDatabaseName("IX_Adjustment_ProductId_CreatedAt");
                
            builder.HasIndex(a => a.CreatedBy)
                .HasDatabaseName("IX_Adjustment_CreatedBy");
        }
    }
}
