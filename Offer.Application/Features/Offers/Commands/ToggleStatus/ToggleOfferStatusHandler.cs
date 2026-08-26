using FluentResults;
using Microsoft.EntityFrameworkCore;
using Offers.Application.Abstractions;
using SharedContracts.Offers.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Offers.Application.Features.Offers.Commands.ToggleStatus;

[Transactional]
public static class ToggleOfferStatusHandler
{
    public static async Task<Result> Handle(
        ToggleOfferStatusCommand command,
        IOffersDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var offer = await context.Offers
            .FirstOrDefaultAsync(o => o.OfferId == command.OfferId, cancellationToken);

        if (offer is null)
        {
            return Result.Fail($"Offer with ID '{command.OfferId}' was not found.");
        }

        bool previousStatus = offer.IsActive;
        bool newStatus = command.IsActive ?? !offer.IsActive;

        if (newStatus)
        {
            offer.Activate();
        }
        else
        {
            offer.Deactivate();
        }

        await context.SaveChangesAsync(cancellationToken);

        if (previousStatus && !newStatus)
        {
            await bus.PublishAsync(new OfferDeactivatedIntegrationEvent(
                OfferId: offer.OfferId,
                OccurredAt: DateTime.UtcNow
            ));
        }

        return Result.Ok();
    }
}
