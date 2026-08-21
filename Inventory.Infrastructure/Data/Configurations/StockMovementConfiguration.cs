using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovements");

            builder.HasKey(s => s.MovementId);

            builder.Property(s => s.MovementId)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder.Property(s => s.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(s => s.Quantity)
                .IsRequired();

            builder.Property(s => s.ReferenceId)
                .IsRequired(false);

            builder.Property(s => s.CreatedAt)
                .IsRequired();


            // Relationships

            builder.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(s => s.ProductBatch)
                .WithMany(s => s.StockMovements)
                .HasForeignKey(s => s.ProductBatchId)
                .OnDelete(DeleteBehavior.Restrict);


            // Normal Indexes

            builder.HasIndex(s => new { s.ProductId, s.CreatedAt })
                .HasDatabaseName("IX_StockMovement_ProductId_CreatedAt");


            builder.HasIndex(s => s.ProductBatchId)
                .HasDatabaseName("IX_StockMovement_ProductBatchId");


            builder.HasIndex(s => s.Type)
                .HasDatabaseName("IX_StockMovement_Type");


            builder.HasIndex(s => s.ReferenceId)
                .HasDatabaseName("IX_StockMovement_ReferenceId");


            // ============================================
            // Idempotency Index
            // Prevent duplicate processing of same receipt
            // ============================================

            builder.HasIndex(s => new
            {
                s.ReferenceId,
                s.ProductId
            })
            .HasDatabaseName("UX_StockMovement_ReferenceId_ProductId")
            .IsUnique();
        }
    }
}