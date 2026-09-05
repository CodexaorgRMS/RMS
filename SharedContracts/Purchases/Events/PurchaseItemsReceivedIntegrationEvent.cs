using System;
using System.Collections.Generic;

namespace SharedContracts.Purchases.Events;

public sealed record PurchaseItemsReceivedIntegrationEvent(
    Guid PurchaseId,
    Guid ReceiptId,
    Guid SupplierId,
    IReadOnlyCollection<PurchaseReceivedItemContract> Items,
    DateTime OccurredAt);

public sealed record PurchaseReceivedItemContract(
    Guid PurchaseReceiptItemId,
    Guid ProductId,
    int Quantity,
    decimal UnitCost,
    DateTime ExpiryDate);