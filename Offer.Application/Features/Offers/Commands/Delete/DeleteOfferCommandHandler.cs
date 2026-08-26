using FluentResults;
using Microsoft.EntityFrameworkCore;
using Offers.Application.Abstractions;
using SharedContracts.Offers.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Offers.Application.Features.Offers.Commands.Delete;

[Transactional]
public static class DeleteOfferCommandHandler
{
    public static async Task<Result> Handle(
        DeleteOfferCommand command,
        IOffersDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var offer = await context.Offers
            .Include(o => o.Targets)
            .FirstOrDefaultAsync(o => o.OfferId == command.OfferId, cancellationToken);

        if (offer is null)
        {
            return Result.Fail($"Offer with ID '{command.OfferId}' was not found.");
        }

        offer.Deactivate();
        await context.SaveChangesAsync(cancellationToken);

        if (offer.IsActive)
        {
            await bus.PublishAsync(new OfferDeactivatedIntegrationEvent(
                OfferId: offer.OfferId,
                OccurredAt: DateTime.UtcNow
            ));
        }

        return Result.Ok();
    }
}
