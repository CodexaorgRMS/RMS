namespace Purchases.Application.Features.Purchases.Commands.Update;

public sealed record UpdatePurchaseCommand(
    Guid PurchaseId,
    Guid SupplierId,
    IReadOnlyCollection<UpdatePurchaseItemDto> Items
);

public sealed record UpdatePurchaseItemDto(
    Guid ProductId,
    int Quantity,
    decimal UnitCost
);