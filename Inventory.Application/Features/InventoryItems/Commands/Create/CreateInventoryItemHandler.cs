using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.Create;

[Transactional]
public static class CreateInventoryItemHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateInventoryItemCommand command,
        IInventoryDbContext context,
        CancellationToken cancellationToken)
    {
        var inventoryItem = new InventoryItem
        {
            InventoryItemId = Guid.NewGuid(),
            ProductId = command.ProductId,
            Quantity = command.Quantity,
            MinStock = command.MinStock,
            UpdatedAt = DateTime.UtcNow
        };

        await context.InventoryItems.AddAsync(
            inventoryItem,
            cancellationToken);

        return Result.Ok(inventoryItem.InventoryItemId);
    }
}