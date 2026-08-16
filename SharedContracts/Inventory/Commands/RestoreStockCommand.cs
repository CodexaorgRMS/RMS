namespace SharedContracts.Inventory.Commands;

public record RestoreStockCommand(Guid ProductId, int Quantity, Guid OrderId);
