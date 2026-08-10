namespace Inventory.Presentation.StockMovements.Requests;

public sealed record CreateStockMovementRequest(
    Guid ProductId,
    string Type,
    int Quantity,
    Guid ReferenceId);