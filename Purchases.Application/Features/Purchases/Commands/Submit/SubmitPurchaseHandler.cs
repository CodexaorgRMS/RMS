using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Purchases.Commands.Submit;

[Transactional(typeof(IPurchasesDataContext))]
public static class SubmitPurchaseHandler
{
    public static async Task<Result<SubmitPurchaseResult>> Handle(
        SubmitPurchaseCommand command,
        IPurchasesDataContext context,
        CancellationToken cancellationToken)
    {
        var purchase = await context.PurchaseOrders
            .Include(x => x.Items)
            .Include(x => x.Supplier)
            .FirstOrDefaultAsync(x => x.PurchaseOrderId == command.PurchaseId, cancellationToken);

        if (purchase is null)
        {
            return Result.Fail("Purchase not found.");
        }

        if (purchase.Status != PurchaseStatus.Draft)
        {
            return Result.Fail("Only Draft purchases can be submitted.");
        }

        if (purchase.Supplier is null || !purchase.Supplier.IsActive)
        {
            return Result.Fail("Supplier is inactive.");
        }

        if (purchase.Items is null || purchase.Items.Count == 0)
        {
            return Result.Fail("Purchase must contain at least one item.");
        }

        if (purchase.Items.Any(i => i.OrderedQuantity <= 0 || i.UnitCost < 0))
        {
            return Result.Fail("Purchase items must have positive quantity and non-negative unit cost.");
        }

        purchase.Status = PurchaseStatus.Ordered;
        purchase.SubmittedAt = DateTime.UtcNow;

        return Result.Ok(new SubmitPurchaseResult(
            purchase.PurchaseOrderId,
            purchase.Status.ToString()));
    }
}
