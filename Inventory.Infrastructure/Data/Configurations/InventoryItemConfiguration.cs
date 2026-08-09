using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(i => i.InventoryItemId);

            builder.Property(i => i.InventoryItemId)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.MinStock)
                .IsRequired();

            builder.Property(i => i.UpdatedAt)
                .IsRequired();

            builder.HasOne(i => i.Product)
                .WithMany(p => p.InventoryItems)
                .HasForeignKey(i => i.ProductId);

            // Perfect Indexes
            builder.HasIndex(i => i.ProductId)
                .HasDatabaseName("IX_InventoryItem_ProductId");
                
            builder.HasIndex(i => new { i.Quantity, i.MinStock })
                .HasDatabaseName("IX_InventoryItem_Quantity_MinStock");
        }
    }
}
