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

        // Validate the purchase order and its items
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
        // Create a map of PurchaseOrderItemId to PurchaseOrderItem for quick lookup 
        // This is done to avoid multiple iterations over the purchase items when validating received quantities.
        // example: if there are 10 items in the purchase order and 10 items in the receipt, without this map,
        // we would have to iterate over the purchase order items 10 times (once for each receipt item) to find the corresponding order item.
        // it solve it by creating a dictionary that allows O(1) lookup time for each receipt item,
        // reducing the overall complexity from O(n*m) to O(n+m).
        var orderItemMap = purchase.Items.ToDictionary(i => i.PurchaseOrderItemId);


        // Validate each received item against the corresponding purchase order item
        // This includes checking if the item exists, if the received quantity is valid, and if the expiry date is in the future.
        foreach (var reqItem in command.Items)
        {
            if (!orderItemMap.TryGetValue(reqItem.PurchaseOrderItemId, out var orderItem))
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
            // Calculate the total quantity already received for this item across all previous receipts
            var alreadyReceived = purchase.Receipts
                .SelectMany(r => r.Items)
                .Where(ri => ri.PurchaseOrderItemId == reqItem.PurchaseOrderItemId)
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
        // Create receipt items and prepare event items for integration event
        // This loop creates a new PurchaseReceiptItem for each received item, linking it to the corresponding PurchaseOrderItem.
        foreach (var reqItem in command.Items)
        {
            var orderItem = orderItemMap[reqItem.PurchaseOrderItemId];

            var receiptItem = new PurchaseReceiptItem
            {
                PurchaseReceiptItemId = Guid.NewGuid(),
                PurchaseReceiptId = receiptId,
                PurchaseOrderItemId = reqItem.PurchaseOrderItemId,
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

        // Determine if all items in the purchase order have been fully received after this receipt
        // This check is crucial for updating the purchase order status correctly.
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

        // Publish the integration event to notify other services of the received items
        await bus.PublishAsync(integrationEvent);

        return Result.Ok(new ReceivePurchaseResult(
            purchase.PurchaseOrderId,
            receiptId,
            purchase.Status.ToString()));
    }
}
