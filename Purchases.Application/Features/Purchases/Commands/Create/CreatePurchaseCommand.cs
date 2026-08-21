namespace Purchases.Application.Features.Purchases.Commands.Create
{
    public sealed record CreatePurchaseCommand(
        Guid SupplierId,
        IReadOnlyCollection<CreatePurchaseItemDto> Items,
        string IdempotencyKey);

    public sealed record CreatePurchaseItemDto(
        Guid ProductId,
        int Quantity,
        decimal UnitCost);
}