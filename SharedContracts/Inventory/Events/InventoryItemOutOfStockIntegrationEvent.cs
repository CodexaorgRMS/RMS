namespace SharedContracts.Inventory.Events;

public sealed record InventoryItemOutOfStockIntegrationEvent(
    Guid InventoryItemId,
    Guid ProductId,
    DateTime OccurredAt);