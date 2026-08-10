namespace SharedContracts.Inventory.Events;

public sealed record InventoryItemRestockedEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int CurrentQuantity,
    int MinStock,
    DateTime OccurredAt
);