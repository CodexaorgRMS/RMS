using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using Purchases.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Purchases.Commands.Create
{
    [Transactional(typeof(IPurchasesDataContext))]
    public static class CreatePurchaseHandler
    {
        private const string OperationName = "CreatePurchase";

        public static async Task<Result<CreatePurchaseResult>> Handle(
            CreatePurchaseCommand command,
            IPurchasesDataContext context)
        {
            var existingRequest = await context.IdempotencyRecords.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Operation == OperationName &&
                    x.IdempotencyKey == command.IdempotencyKey);

            if (existingRequest is not null)
            {
                var existingPurchase = await context.PurchaseOrders.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PurchaseOrderId == existingRequest.ResourceId);

                return Result.Ok(new CreatePurchaseResult(
                    existingRequest.ResourceId,
                    (existingPurchase?.Status ?? PurchaseStatus.Draft).ToString()));
            }

            var purchaseOrderId = Guid.NewGuid();

            var items = command.Items
                .Select(item => new PurchaseOrderItem
                {
                    PurchaseOrderItemId = Guid.NewGuid(),
                    PurchaseOrderId = purchaseOrderId,
                    ProductId = item.ProductId,
                    OrderedQuantity = item.Quantity,
                    UnitCost = item.UnitCost
                })
                .ToList();

            
            var totalAmount = items.Sum(i => i.OrderedQuantity * i.UnitCost);

            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderId = purchaseOrderId,
                SupplierId = command.SupplierId,
                Status = PurchaseStatus.Draft,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.UtcNow,
                Items = items
            };

            var idempotencyRecord = new IdempotencyRecord
            {
                IdempotencyRecordId = Guid.NewGuid(),
                IdempotencyKey = command.IdempotencyKey,
                Operation = OperationName,
                ResourceId = purchaseOrderId,
                CreatedAt = DateTime.UtcNow
            };

            context.PurchaseOrders.Add(purchaseOrder);
            context.IdempotencyRecords.Add(idempotencyRecord);

            return Result.Ok(new CreatePurchaseResult(
                purchaseOrderId,
                PurchaseStatus.Draft.ToString()));
        }
    }
}