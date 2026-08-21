using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Purchases.Commands.Cancel;

[Transactional(typeof(IPurchasesDataContext))]
public static class CancelPurchaseHandler
{
    public static async Task<Result<CancelPurchaseResult>> Handle(
        CancelPurchaseCommand command,
        IPurchasesDataContext context,
        CancellationToken cancellationToken)
    {
        var purchase = await context.PurchaseOrders
            .FirstOrDefaultAsync(x => x.PurchaseOrderId == command.PurchaseId, cancellationToken);

        if (purchase is null)
        {
            return Result.Fail("Purchase not found.");
        }

        if (purchase.Status != PurchaseStatus.Draft && purchase.Status != PurchaseStatus.Ordered)
        {
            return Result.Fail($"Cannot cancel a purchase with status {purchase.Status}.");
        }

        purchase.Status = PurchaseStatus.Cancelled;
        purchase.CancelledAt = DateTime.UtcNow;

        return Result.Ok(new CancelPurchaseResult(
            purchase.PurchaseOrderId,
            purchase.Status.ToString()));
    }
}
