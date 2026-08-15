using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.ProductBatches.Commands.ChangeStatus;

[Transactional]
public static class ChangeProductBatchStatusHandler
{
    public static async Task<Result> Handle(
        ChangeProductBatchStatusCommand command,
        IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
        var batch = await context.ProductBatches
            .FirstOrDefaultAsync(b => b.BatchId == command.BatchId, cancellationToken);

        if (batch is null)
        {
            return Result.Fail($"Product batch with ID '{command.BatchId}' was not found.");
        }

        batch.Status = command.Status;
        batch.HoldReason = command.HoldReason;
        batch.StatusChangedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}