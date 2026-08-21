namespace Purchases.Application.Features.Purchases.Commands.Update;

public sealed record UpdatePurchaseResult(
    Guid PurchaseOrderId,
    Guid SupplierId,
    string Status,
    decimal TotalAmount,
    IReadOnlyCollection<UpdatePurchaseItemResult> Items
);
public sealed record UpdatePurchaseItemResult(
    Guid ProductId,
    int Quantity,
    decimal UnitCost
);