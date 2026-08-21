using System;
using System.Collections.Generic;

namespace Purchases.Presentation.Requests
{
    public sealed record UpdatePurchaseRequest(
        Guid SupplierId,
        IReadOnlyCollection<UpdatePurchaseItemRequest> Items
    );

    public sealed record UpdatePurchaseItemRequest(
        Guid ProductId,
        int Quantity,
        decimal UnitCost
    );
}