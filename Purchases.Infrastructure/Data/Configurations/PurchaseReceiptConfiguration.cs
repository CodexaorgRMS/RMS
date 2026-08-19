using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Data.Configurations;

public sealed class PurchaseReceiptConfiguration
    : IEntityTypeConfiguration<PurchaseReceipt>
{
    public void Configure(EntityTypeBuilder<PurchaseReceipt> builder)
    {
        builder.ToTable("PurchaseReceipts");

        builder.HasKey(x => x.PurchaseReceiptId);

        builder.Property(x => x.PurchaseReceiptId)
            .ValueGeneratedNever();

        builder.Property(x => x.PurchaseOrderId)
            .IsRequired();

        builder.Property(x => x.ReceivedAt)
            .IsRequired();

        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(x => x.Receipts)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.PurchaseReceipt)
            .HasForeignKey(x => x.PurchaseReceiptId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.PurchaseOrderId);

        builder.HasIndex(x => x.ReceivedAt);
    }
}