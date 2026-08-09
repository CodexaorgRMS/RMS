using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.HasKey(s => s.MovementId);

            builder.Property(s => s.MovementId)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder.Property(s => s.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Quantity)
                .IsRequired();

            builder.Property(s => s.ReferenceId)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.HasOne(s => s.Product)
                .WithMany(p => p.StockMovements)
                .HasForeignKey(s => s.ProductId);

            // Perfect Indexes
            builder.HasIndex(s => new { s.ProductId, s.CreatedAt })
                .HasDatabaseName("IX_StockMovement_ProductId_CreatedAt");
                
            builder.HasIndex(s => s.Type)
                .HasDatabaseName("IX_StockMovement_Type");
        }
    }
}
