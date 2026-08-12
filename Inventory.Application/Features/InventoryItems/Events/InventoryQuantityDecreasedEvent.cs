namespace Inventory.Application.Features.InventoryItems.Events;

public sealed record InventoryQuantityDecreasedEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int Quantity,
    DateTime OccurredAt);