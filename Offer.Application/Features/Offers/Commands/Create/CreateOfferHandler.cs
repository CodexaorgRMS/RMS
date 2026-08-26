using FluentResults;
using Offers.Application.Abstractions;
using Offers.Domain.Entities;
using SharedContracts.Offers.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Offers.Application.Features.Offers.Commands.Create;

[Transactional]
public static class CreateOfferHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateOfferCommand command,
        IOffersDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var offer = new Offer
        {
            OfferId = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Type = command.Type,
            Value = command.Value,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Priority = command.Priority,
            IsSmart = command.IsSmart,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        if (command.Targets is { Count: > 0 })
        {
            foreach (var target in command.Targets)
            {
                offer.Targets.Add(new OfferTarget
                {
                    OfferTargetId = Guid.NewGuid(),
                    OfferId = offer.OfferId,
                    TargetType = target.TargetType,
                    TargetId = target.TargetId,
                    RequiredQuantity = target.RequiredQuantity,
                    SpecialPrice = target.SpecialPrice
                });
            }
        }

        await context.Offers.AddAsync(offer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var integrationEvent = new OfferCreatedIntegrationEvent(
            OfferId: offer.OfferId,
            Name: offer.Name,
            Type: offer.Type.ToString(),
            Value: offer.Value,
            StartDate: offer.StartDate,
            EndDate: offer.EndDate,
            OccurredAt: DateTime.UtcNow
        );

        await bus.PublishAsync(integrationEvent);

        return Result.Ok(offer.OfferId);
    }
}
