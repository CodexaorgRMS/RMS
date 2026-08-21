using System;
using System.Collections.Generic;

namespace Purchases.Presentation.Requests
{
    public sealed record CreatePurchaseRequest(
        Guid SupplierId,
        IReadOnlyCollection<CreatePurchaseItemRequest> Items
    );

    public sealed record CreatePurchaseItemRequest(
        Guid ProductId,
        int Quantity,
        decimal UnitCost
    );
}