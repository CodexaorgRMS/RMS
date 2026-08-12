namespace Inventory.Application.Features.InventoryItems.Events;

public sealed record InventoryQuantityIncreasedEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int Quantity,
    DateTime OccurredAt);