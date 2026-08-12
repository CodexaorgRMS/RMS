using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
    {
        public void Configure(EntityTypeBuilder<ProductBatch> builder)
        {
            builder.HasKey(p => p.BatchId);

            builder.Property(p => p.BatchId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ProductId)
                .IsRequired();

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.ExpiryDate)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(pr => pr.ProductBatches)
                .HasForeignKey(p => p.ProductId);

            // Perfect Indexes
            builder.HasIndex(p => new { p.ProductId, p.ExpiryDate })
                .HasDatabaseName("IX_ProductBatch_ProductId_ExpiryDate");

            builder.HasIndex(p => p.ExpiryDate)
                .HasDatabaseName("IX_ProductBatch_ExpiryDate");

            builder.HasOne(p => p.Product)
                .WithMany(pr => pr.ProductBatches)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
