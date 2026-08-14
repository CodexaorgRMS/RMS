using System;

namespace Inventory.Application.Features.ProductBatches.Events
{
    public sealed record ProductBatchReceivedEvent(
        Guid BatchId,
        Guid ProductId,
        decimal CostPrice,
        int Quantity,
        DateTime ExpiryDate,
        DateTime ReceivedAt);
}
