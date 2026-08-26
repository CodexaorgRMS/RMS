using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using Purchases.Domain.Enums;
using SharedContracts.Inventory.Events;
using Wolverine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Purchases.Application.Features.Purchases.EventHandlers;

public static class LowStockDetectedEventHandler
{
    [Transactional]
    public static async Task Handle(
        LowStockDetectedIntegrationEvent @event,
        IPurchasesDataContext context,
        CancellationToken cancellationToken)
    {
        var existingDraft = context.PurchaseOrderItems
            .Any(poi => poi.ProductId == @event.ProductId && poi.PurchaseOrder.Status == PurchaseStatus.Draft);

        if (existingDraft) return;

        var defaultSupplierId = Guid.Empty;

        var purchaseOrder = new PurchaseOrder
        {
            PurchaseOrderId = Guid.NewGuid(),
            SupplierId = defaultSupplierId,
            Status = PurchaseStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            Items = new List<PurchaseOrderItem>
            {
                new PurchaseOrderItem
                {
                    PurchaseOrderItemId = Guid.NewGuid(),
                    ProductId = @event.ProductId,
                    OrderedQuantity = @event.MinStock * 2,
                    UnitCost = 0
                }
            }
        };

        await context.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
    }
}
