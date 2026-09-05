namespace SharedContracts.Inventory.Events;

public sealed record StockAdjustedIntegrationEvent(
    Guid AdjustmentId,
    Guid ProductId,
    int Quantity,
    decimal UnitCost,
    string AdjustmentType,
    DateTime OccurredAt);