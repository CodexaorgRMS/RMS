using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class AdjustmentConfiguration : IEntityTypeConfiguration<Adjustment>
    {
        public void Configure(EntityTypeBuilder<Adjustment> builder)
        {
            builder.ToTable("Adjustments");

            builder.HasKey(a => a.AdjustmentId);

            builder.Property(a => a.AdjustmentId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.ProductId)
                .IsRequired();

            builder.Property(a => a.ProductBatchId)
                .IsRequired();

            builder.Property(a => a.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(a => a.Reason)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(a => a.Quantity)
                .IsRequired();

            builder.Property(a => a.TotalFinancialImpact)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(a => a.Note)
                .HasMaxLength(500);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(a => a.Product)
                .WithMany(p => p.Adjustments)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ProductBatch)
                .WithMany()
                .HasForeignKey(a => a.ProductBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Perfect Indexes
            builder.HasIndex(a => new { a.ProductId, a.CreatedAt })
                .HasDatabaseName("IX_Adjustment_ProductId_CreatedAt");

            builder.HasIndex(a => a.ProductBatchId)
                .HasDatabaseName("IX_Adjustment_ProductBatchId");

            builder.HasIndex(a => a.Type)
                .HasDatabaseName("IX_Adjustment_Type");

            builder.HasIndex(a => a.Reason)
                .HasDatabaseName("IX_Adjustment_Reason");
        }
    }
}
