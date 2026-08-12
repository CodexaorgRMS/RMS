namespace SharedContracts.Inventory.Events;

public sealed record InventoryItemRestockedIntegrationEvent(
    Guid InventoryItemId,
    Guid ProductId,
    int CurrentQuantity,
    int MinStock,
    DateTime OccurredAt);