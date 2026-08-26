namespace Offers.Application.Features.Offers.Commands.ToggleStatus;

public sealed record ToggleOfferStatusCommand(
    Guid OfferId,
    bool? IsActive = null);
