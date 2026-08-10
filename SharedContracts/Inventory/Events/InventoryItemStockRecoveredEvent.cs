namespace SharedContracts.Inventory.Events;

public sealed record InventoryItemStockRecoveredEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int CurrentQuantity,
    int MinStock,
    DateTime OccurredAt
);