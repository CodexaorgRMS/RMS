namespace SharedContracts.Inventory.Events;

public sealed record LowStockDetectedEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int CurrentQuantity,
    int MinStock,
    DateTime OccurredAt
);