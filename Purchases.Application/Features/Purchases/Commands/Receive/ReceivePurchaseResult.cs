using System;

namespace Purchases.Application.Features.Purchases.Commands.Receive;

public record ReceivePurchaseResult(
    Guid PurchaseOrderId,
    Guid ReceiptId,
    string Status);
