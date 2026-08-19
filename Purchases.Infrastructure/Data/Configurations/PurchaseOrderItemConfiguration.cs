using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Data.Configurations;

public sealed class PurchaseOrderItemConfiguration
    : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.ToTable("PurchaseOrderItems");

        builder.HasKey(x => x.PurchaseOrderItemId);

        builder.Property(x => x.PurchaseOrderItemId)
            .ValueGeneratedNever();

        builder.Property(x => x.PurchaseOrderId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.OrderedQuantity)
            .IsRequired();

        builder.Property(x => x.UnitCost)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ReceiptItems)
            .WithOne(x => x.PurchaseOrderItem)
            .HasForeignKey(x => x.PurchaseOrderItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ProductId);

        builder.HasIndex(x => new
        {
            x.PurchaseOrderId,
            x.ProductId
        })
        .IsUnique();
    }
}