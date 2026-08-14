using System;

namespace Inventory.Application.Features.ProductBatches.Commands.Receive
{
    public record ReceiveProductBatchCommand(
        Guid ProductId,
        decimal CostPrice,
        int Quantity,
        DateTime ExpiryDate);
}
