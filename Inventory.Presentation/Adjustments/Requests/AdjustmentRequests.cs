using Inventory.Domain.Entities;
using System;

namespace Inventory.Presentation.Adjustments.Requests
{
    public record CreateAdjustmentRequest(
        Guid ProductId,
        Guid ProductBatchId,
        AdjustmentType Type,
        AdjustmentReason Reason,
        int Quantity,
        string? Note);
}
