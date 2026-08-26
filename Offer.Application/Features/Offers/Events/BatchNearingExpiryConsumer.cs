using FluentResults;
using Offers.Application.Abstractions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using SharedContracts.Inventory.Events;
using SharedContracts.Offers.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Offers.Application.Features.Offers.Events;

[Transactional]
public static class BatchNearingExpiryConsumer
{
    public static async Task Handle(
        BatchNearingExpiryIntegrationEvent @event,
        IOffersDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        decimal discountPercentage = @event.SuggestedDiscountPercentage > 0
            ? @event.SuggestedDiscountPercentage
            : 20m;

        var smartOffer = new Offer
        {
            OfferId = Guid.NewGuid(),
            Name = $"Smart Markdown: {@event.ProductName} (Expiry in {@event.RemainingDays}d)",
            Description = $"Auto-generated markdown for expiring batch {@event.BatchId} (Barcode: {@event.Barcode}).",
            Type = OfferType.Percentage,
            Value = discountPercentage,
            StartDate = now,
            EndDate = @event.ExpiryDate > now ? @event.ExpiryDate : now.AddDays(1),
            Priority = 100, // High priority for clearance / expiring items
            IsSmart = true,
            IsActive = true,
            CreatedAt = now
        };

        smartOffer.Targets.Add(new OfferTarget
        {
            OfferTargetId = Guid.NewGuid(),
            OfferId = smartOffer.OfferId,
            TargetType = OfferTargetType.Product,
            TargetId = @event.ProductId,
            RequiredQuantity = 1,
            SpecialPrice = null
        });

        await context.Offers.AddAsync(smartOffer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var integrationEvent = new OfferCreatedIntegrationEvent(
            OfferId: smartOffer.OfferId,
            Name: smartOffer.Name,
            Type: smartOffer.Type.ToString(),
            Value: smartOffer.Value,
            StartDate: smartOffer.StartDate,
            EndDate: smartOffer.EndDate,
            OccurredAt: now
        );

        await bus.PublishAsync(integrationEvent);
    }
}
