using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Adjustments.Commands.Create;

[Transactional]
public static class CreateAdjustmentHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateAdjustmentCommand command,
        IInventoryDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var batch = await context.ProductBatches
            .FirstOrDefaultAsync(
                b => b.BatchId == command.ProductBatchId,
                cancellationToken);

        if (batch is null)
        {
            return Result.Fail("Product batch not found.");
        }


        if (command.Type == AdjustmentType.Increase)
        {
            batch.CurrentQuantity += command.Quantity;
        }
        else
        {
            if (batch.CurrentQuantity < command.Quantity)
            {
                return Result.Fail("Not enough quantity in batch.");
            }

            batch.CurrentQuantity -= command.Quantity;
        }


        var totalFinancialImpact = command.Quantity * batch.CostPrice;


        var adjustment = new Adjustment
        {
            AdjustmentId = Guid.NewGuid(),
            ProductId = command.ProductId,
            ProductBatchId = command.ProductBatchId,
            Type = command.Type,
            Reason = command.Reason,
            Quantity = command.Quantity,
            TotalFinancialImpact = totalFinancialImpact,
            Note = command.Note,
            CreatedAt = DateTime.UtcNow
        };


        await context.Adjustments.AddAsync(
            adjustment,
            cancellationToken);



        var inventoryItem = await context.InventoryItems
            .FirstOrDefaultAsync(
                i => i.ProductId == command.ProductId,
                cancellationToken);


        if (inventoryItem is null)
        {
            return Result.Fail(
                $"Inventory item with ProductId {command.ProductId} not found.");
        }


        if (command.Type == AdjustmentType.Increase)
        {
            inventoryItem.Quantity += command.Quantity;
        }
        else
        {
            inventoryItem.Quantity -= command.Quantity;
        }

        inventoryItem.UpdatedAt = DateTime.UtcNow;



        var movementType = command.Type == AdjustmentType.Increase
            ? StockMovementType.In
            : StockMovementType.Out;


        var movement = new StockMovement
        {
            MovementId = Guid.NewGuid(),
            ProductId = command.ProductId,
            ProductBatchId = command.ProductBatchId,
            Quantity = command.Quantity,
            Type = movementType,
            CreatedAt = DateTime.UtcNow,
            ReferenceId = adjustment.AdjustmentId
        };


        await context.StockMovements.AddAsync(
            movement,
            cancellationToken);



        await context.SaveChangesAsync(cancellationToken);



        var eventQuantity = command.Type == AdjustmentType.Decrease
            ? -command.Quantity
            : command.Quantity;


        var stockAdjustedEvent = new StockAdjustedIntegrationEvent(
            adjustment.AdjustmentId,
            adjustment.ProductId,
            eventQuantity,
            batch.CostPrice,
            adjustment.Reason.ToString(),
            adjustment.CreatedAt);


        await bus.PublishAsync(
            stockAdjustedEvent);


        return Result.Ok(adjustment.AdjustmentId);
    }
}