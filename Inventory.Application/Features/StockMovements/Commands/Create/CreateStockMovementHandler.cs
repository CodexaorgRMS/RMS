using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;
using Wolverine.Attributes;
using SharedContracts.Inventory.Events;
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
            .FirstOrDefaultAsync(x => x.ProductId == command.ProductId, cancellationToken);

        if (inventoryItem is null)
        {
            return Result.Fail<Guid>("Inventory item not found for this product.");
        }

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

        if (command.Type == "IN")
        {
            inventoryItem.Quantity += command.Quantity;
        }
        else if (command.Type is "OUT" or "ADJUST")
        {
            inventoryItem.Quantity -= command.Quantity;
        }
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        var @event = new StockMovementCreatedEvent(
            movement.MovementId,
            movement.ProductId,
            movement.Type,
            movement.Quantity,
            movement.ReferenceId,
            movement.CreatedAt
        );

   

		await bus.PublishAsync(@event);

        return Result.Ok(movement.MovementId);
    }
}