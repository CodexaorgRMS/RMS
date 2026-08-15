using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.ProductBatches.Commands.CheckExpiring;

[Transactional]
public static class CheckExpiringBatchesHandler
{
    public static async Task<Result> Handle(
        CheckExpiringBatchesCommand command,
        IInventoryDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // Fetch all active batches with remaining stock along with their Product & Category configurations
        var activeBatches = await context.ProductBatches
            .Include(b => b.Product)
            .ThenInclude(p => p.Category)
            .Where(b => b.Status == BatchStatus.Active && b.CurrentQuantity > 0)
            .ToListAsync(cancellationToken);

        foreach (var batch in activeBatches)
        {
            var product = batch.Product;
            var category = product.Category;

            // 1. Expired batches: Auto-mark as Expired to quarantine from sales immediately
            if (batch.ExpiryDate <= now)
            {
                batch.Status = BatchStatus.Expired;
                batch.HoldReason = "Automatically expired by the system.";
                batch.StatusChangedAt = now;
                continue;
            }

            // 2. Hierarchy Fallback: Product Override ?? Category Default
            int warningDays = product.CustomExpiryWarningDays ?? category.ExpiryWarningDays;
            decimal markdownPercentage = product.CustomAutoMarkdownPercentage ?? category.AutoMarkdownPercentage;

            var thresholdDate = now.AddDays(warningDays);

            // 3. Check if batch falls within the warning threshold
            if (batch.ExpiryDate <= thresholdDate)
            {
                int remainingDays = (int)Math.Ceiling((batch.ExpiryDate - now).TotalDays);

                var integrationEvent = new BatchNearingExpiryIntegrationEvent(
                    BatchId: batch.BatchId,
                    ProductId: product.ProductId,
                    ProductName: product.Name,
                    Barcode: product.Barcode,
                    ExpiryDate: batch.ExpiryDate,
                    RemainingDays: remainingDays,
                    CurrentQuantity: batch.CurrentQuantity,
                    SuggestedDiscountPercentage: markdownPercentage,
                    OccurredAt: now
                );

                // Publish event to shared bus (Subscribed by Notifications & Offers modules)
                await bus.PublishAsync(integrationEvent);
            }
        }

        return Result.Ok();
    }
}