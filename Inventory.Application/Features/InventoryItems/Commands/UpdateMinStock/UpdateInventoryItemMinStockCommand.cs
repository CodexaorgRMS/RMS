namespace Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock;

public sealed record UpdateInventoryItemMinStockCommand(
    Guid InventoryItemId,
    int MinStock
);