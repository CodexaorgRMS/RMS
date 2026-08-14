using System;

namespace Inventory.Presentation.ProductBatches.Requests
{
    public record ReceiveProductBatchRequest(
        Guid ProductId,
        decimal CostPrice,
        int Quantity,
        DateTime ExpiryDate);
}
