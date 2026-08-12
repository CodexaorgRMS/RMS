namespace SharedContracts.Inventory.Events;

public sealed record LowStockDetectedIntegrationEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int CurrentQuantity,
    int MinStock,
    DateTime OccurredAt);