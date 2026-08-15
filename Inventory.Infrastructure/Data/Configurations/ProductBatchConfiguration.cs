using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
    {
        public void Configure(EntityTypeBuilder<ProductBatch> builder)
        {
            builder.ToTable("ProductBatches");

            builder.HasKey(p => p.BatchId);

            builder.Property(p => p.BatchId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ProductId)
                .IsRequired();

            builder.Property(p => p.CostPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.InitialQuantity)
                .IsRequired();

            builder.Property(p => p.CurrentQuantity)
                .IsRequired();

            builder.Property(p => p.ExpiryDate)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .HasDefaultValue(BatchStatus.Active)
                .IsRequired();

            builder.Property(p => p.HoldReason)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.StatusChangedAt)
                .IsRequired(false);

            builder.HasOne(p => p.Product)
                .WithMany(pr => pr.ProductBatches)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Perfect Indexes
            builder.HasIndex(p => new { p.ProductId, p.ExpiryDate })
                .HasDatabaseName("IX_ProductBatch_ProductId_ExpiryDate");

            builder.HasIndex(p => p.ExpiryDate)
                .HasDatabaseName("IX_ProductBatch_ExpiryDate");

            builder.HasIndex(p => new { p.ProductId, p.CurrentQuantity })
                .HasDatabaseName("IX_ProductBatch_ProductId_CurrentQuantity");

            builder.HasIndex(p => new { p.ProductId, p.Status, p.CurrentQuantity })
                .HasDatabaseName("IX_ProductBatch_ProductId_Status_Quantity");
        }
    }
}
