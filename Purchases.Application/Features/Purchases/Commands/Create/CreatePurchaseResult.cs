using System;

namespace Purchases.Application.Features.Purchases.Commands.Create
{
    public sealed record CreatePurchaseResult(
        Guid PurchaseOrderId,
        string Status);
}