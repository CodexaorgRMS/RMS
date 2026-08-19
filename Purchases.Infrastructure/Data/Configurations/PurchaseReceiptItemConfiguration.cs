using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Data.Configurations;

public sealed class PurchaseReceiptItemConfiguration
    : IEntityTypeConfiguration<PurchaseReceiptItem>
{
    public void Configure(EntityTypeBuilder<PurchaseReceiptItem> builder)
    {
        builder.ToTable("PurchaseReceiptItems");

        builder.HasKey(x => x.PurchaseReceiptItemId);

        builder.Property(x => x.PurchaseReceiptItemId)
            .ValueGeneratedNever();

        builder.Property(x => x.PurchaseReceiptId)
            .IsRequired();

        builder.Property(x => x.PurchaseOrderItemId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.ReceivedQuantity)
            .IsRequired();

        builder.Property(x => x.ExpiryDate);

        builder.HasOne(x => x.PurchaseReceipt)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PurchaseReceiptId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PurchaseOrderItem)
            .WithMany(x => x.ReceiptItems)
            .HasForeignKey(x => x.PurchaseOrderItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.PurchaseReceiptId);

        builder.HasIndex(x => x.PurchaseOrderItemId);

        builder.HasIndex(x => x.ProductId);
    }
}