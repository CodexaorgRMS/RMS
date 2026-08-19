using Microsoft.EntityFrameworkCore;
using Purchases.Domain.Entities;

namespace Purchases.Application.Abstractions;

public interface IPurchasesDataContext
{
    DbSet<Supplier> Suppliers { get; }

    DbSet<PurchaseOrder> PurchaseOrders { get; }

    DbSet<PurchaseOrderItem> PurchaseOrderItems { get; }

    DbSet<PurchaseReceipt> PurchaseReceipts { get; }

    DbSet<PurchaseReceiptItem> PurchaseReceiptItems { get; }

    DbSet<IdempotencyRecord> IdempotencyRecords { get; }
}