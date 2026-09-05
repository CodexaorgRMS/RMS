using System;
using System.Collections.Generic;

namespace Purchases.Presentation.Requests;

public record ReceivePurchaseRequest(List<ReceivePurchaseItemRequest> Items);

public record ReceivePurchaseItemRequest(
    Guid PurchaseOrderItemId,
    int ReceivedQuantity,
    DateTime ExpiryDate);
