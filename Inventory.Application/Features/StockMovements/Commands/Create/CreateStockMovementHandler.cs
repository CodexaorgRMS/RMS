using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.StockMovements.Commands.Create;

[Transactional]
public static class CreateStockMovementHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateStockMovementCommand command,
        IInventoryDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await context.InventoryItems
            .FirstAsync(x => x.ProductId == command.ProductId, cancellationToken);

        var movement = new StockMovement
        {
            MovementId = Guid.NewGuid(),
            ProductId = command.ProductId,
            Type = command.Type,
            Quantity = command.Quantity,
            ReferenceId = command.ReferenceId,
            CreatedAt = DateTime.UtcNow
        };

        await context.StockMovements.AddAsync(movement, cancellationToken);

        if (command.Type == StockMovementType.In)
        {
            inventoryItem.Quantity += command.Quantity;
        }
        else if (command.Type is StockMovementType.Out or StockMovementType.Adjustment)
        {
            inventoryItem.Quantity -= command.Quantity;
        }
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        var @event = new StockMovementCreatedEvent(
            movement.MovementId,
            movement.ProductId,
            movement.Type.ToString(),
            movement.Quantity,
            movement.ReferenceId ?? Guid.Empty,
            movement.CreatedAt
        );

        await bus.PublishAsync(@event);

        return Result.Ok(movement.MovementId);
    }
}