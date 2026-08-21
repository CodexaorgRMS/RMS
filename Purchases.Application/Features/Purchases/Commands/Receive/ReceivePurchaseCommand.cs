using System;
using System.Collections.Generic;

namespace Purchases.Application.Features.Purchases.Commands.Receive;

public record ReceivePurchaseCommand(
    Guid PurchaseId,
    List<ReceivePurchaseItemDto> Items);

public record ReceivePurchaseItemDto(
    Guid PurchaseItemId,
    int ReceivedQuantity,
    DateTime ExpiryDate);
