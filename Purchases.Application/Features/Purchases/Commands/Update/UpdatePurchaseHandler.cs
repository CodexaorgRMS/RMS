using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using Purchases.Domain.Enums;
using Wolverine;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Purchases.Commands.Update;

public static class UpdatePurchaseHandler
{
    [Transactional(typeof(IPurchasesDataContext))]
    public static async Task<Result<UpdatePurchaseResult>> Handle(
        UpdatePurchaseCommand command,
        IPurchasesDataContext context,
        CancellationToken cancellationToken)
    {
        var purchase = await context.PurchaseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.PurchaseOrderId == command.PurchaseId,
                cancellationToken);

        if (purchase is null)
        {
            return Result.Fail("Purchase not found.");
        }

        if (purchase.Status != PurchaseStatus.Draft)
        {
            return Result.Fail("Only Draft purchases can be updated.");
        }

        purchase.SupplierId = command.SupplierId;

        // امسح بس المنتجات اللي اتشالت من الطلب الجديد
        var newProductIds = command.Items.Select(i => i.ProductId).ToHashSet();

        var itemsToRemove = purchase.Items
            .Where(i => !newProductIds.Contains(i.ProductId))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            purchase.Items.Remove(item);
        }

        // حدّث الموجود، وضيف الجديد بس
        foreach (var newItem in command.Items)
        {
            var existingItem = purchase.Items
                .FirstOrDefault(i => i.ProductId == newItem.ProductId);

            if (existingItem is not null)
            {
                existingItem.OrderedQuantity = newItem.Quantity;
                existingItem.UnitCost = newItem.UnitCost;
            }
            else
            {
                purchase.Items.Add(new PurchaseOrderItem
                {
                    PurchaseOrderItemId = Guid.NewGuid(),
                    PurchaseOrderId = purchase.PurchaseOrderId,
                    ProductId = newItem.ProductId,
                    OrderedQuantity = newItem.Quantity,
                    UnitCost = newItem.UnitCost
                });
            }
        }

        purchase.TotalAmount = purchase.Items.Sum(i => i.OrderedQuantity * i.UnitCost);

        //return Result.Ok(
        //    new UpdatePurchaseResult(
        //        purchase.PurchaseOrderId,
        //        purchase.Status.ToString()));
        return Result.Ok(new UpdatePurchaseResult(
            PurchaseOrderId: purchase.PurchaseOrderId,
            SupplierId: purchase.SupplierId,
            Status: purchase.Status.ToString(),
            TotalAmount: purchase.TotalAmount,
            Items: purchase.Items.Select(i => new UpdatePurchaseItemResult(
                ProductId: i.ProductId,
                Quantity: i.OrderedQuantity,
                UnitCost: i.UnitCost
            )).ToList()
        ));
    }
}