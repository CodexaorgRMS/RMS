using Inventory.Domain.Entities;
using System;

namespace Inventory.Application.Features.Adjustments.Commands.Create
{
    public record CreateAdjustmentCommand(
        Guid ProductId,
        Guid ProductBatchId,
        AdjustmentType Type,
        AdjustmentReason Reason,
        int Quantity,
        string? Note);
}
