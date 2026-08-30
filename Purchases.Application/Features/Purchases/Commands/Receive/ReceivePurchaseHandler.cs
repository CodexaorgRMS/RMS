using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using Purchases.Domain.Enums;
using SharedContracts.Purchases.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Purchases.Commands.Receive;

[Transactional(typeof(IPurchasesDataContext))]
public static class ReceivePurchaseHandler
{
    public static async Task<Result<ReceivePurchaseResult>> Handle(
        ReceivePurchaseCommand command,
        IPurchasesDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var purchase = await context.PurchaseOrders
            .Include(x => x.Items)
            .Include(x => x.Receipts)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(x => x.PurchaseOrderId == command.PurchaseId, cancellationToken);

        if (purchase is null)
        {
            return Result.Fail("Purchase not found.");
        }

        if (purchase.Status == PurchaseStatus.Draft)
        {
            return Result.Fail("Cannot receive a purchase in Draft status.");
        }

        if (purchase.Status == PurchaseStatus.Cancelled)
        {
            return Result.Fail("Cannot receive a cancelled purchase.");
        }

        if (purchase.Status == PurchaseStatus.Received)
        {
            return Result.Fail("Purchase is already fully received.");
        }

        var orderItemMap = purchase.Items.ToDictionary(i => i.PurchaseOrderItemId);

        // Pre-validate all items before making state mutations
        foreach (var reqItem in command.Items)
        {
            if (!orderItemMap.TryGetValue(reqItem.PurchaseItemId, out var orderItem))
            {
                return Result.Fail("PurchaseOrderItem not found in this purchase.");
            }

            if (reqItem.ReceivedQuantity <= 0)
            {
                return Result.Fail("Received quantity must be greater than zero.");
            }

            if (reqItem.ExpiryDate <= DateTime.UtcNow)
            {
                return Result.Fail("Expiry date must be in the future.");
            }

            var alreadyReceived = purchase.Receipts
                .SelectMany(r => r.Items)
                .Where(ri => ri.PurchaseOrderItemId == reqItem.PurchaseItemId)
                .Sum(ri => ri.ReceivedQuantity);

            var remaining = orderItem.OrderedQuantity - alreadyReceived;

            if (reqItem.ReceivedQuantity > remaining)
            {
                return Result.Fail($"Cannot receive more than remaining quantity. Remaining: {remaining}.");
            }
        }

        var receiptId = Guid.NewGuid();
        var receipt = new PurchaseReceipt
        {
            PurchaseReceiptId = receiptId,
            PurchaseOrderId = purchase.PurchaseOrderId,
            ReceivedAt = DateTime.UtcNow,
            Items = new List<PurchaseReceiptItem>()
        };

        var eventItems = new List<PurchaseReceivedItemContract>();

        foreach (var reqItem in command.Items)
        {
            var orderItem = orderItemMap[reqItem.PurchaseItemId];

            var receiptItem = new PurchaseReceiptItem
            {
                PurchaseReceiptItemId = Guid.NewGuid(),
                PurchaseReceiptId = receiptId,
                PurchaseOrderItemId = reqItem.PurchaseItemId,
                ProductId = orderItem.ProductId,
                ReceivedQuantity = reqItem.ReceivedQuantity,
                ExpiryDate = reqItem.ExpiryDate
            };

            receipt.Items.Add(receiptItem);

            eventItems.Add(new PurchaseReceivedItemContract(
                orderItem.ProductId,
                reqItem.ReceivedQuantity,
                orderItem.UnitCost,
                reqItem.ExpiryDate));
        }

        // Determine updated status
        var allFullyReceived = purchase.Items.All(item =>
        {
            var previouslyReceived = purchase.Receipts
                .SelectMany(r => r.Items)
                .Where(ri => ri.PurchaseOrderItemId == item.PurchaseOrderItemId)
                .Sum(ri => ri.ReceivedQuantity);

            var newlyReceived = receipt.Items
                .Where(ri => ri.PurchaseOrderItemId == item.PurchaseOrderItemId)
                .Sum(ri => ri.ReceivedQuantity);

            return (previouslyReceived + newlyReceived) >= item.OrderedQuantity;
        });

        if (allFullyReceived)
        {
            purchase.Status = PurchaseStatus.Received;
            purchase.CompletedAt = DateTime.UtcNow;
        }
        else
        {
            purchase.Status = PurchaseStatus.PartiallyReceived;
        }

        // Force RowVersion update for concurrency protection
        context.PurchaseOrders.Update(purchase);

        context.PurchaseReceipts.Add(receipt);

        var integrationEvent = new PurchaseItemsReceivedIntegrationEvent(
            purchase.PurchaseOrderId,
            receiptId,
            purchase.SupplierId,
            eventItems,
            DateTime.UtcNow);

        await bus.PublishAsync(integrationEvent);

        return Result.Ok(new ReceivePurchaseResult(
            purchase.PurchaseOrderId,
            receiptId,
            purchase.Status.ToString()));
    }
}
