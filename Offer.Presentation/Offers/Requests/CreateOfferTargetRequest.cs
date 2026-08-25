using Offers.Domain.Enums;

namespace Offers.Presentation.Offers.Requests;

public sealed record CreateOfferTargetRequest(
    OfferTargetType TargetType,
    Guid TargetId,
    int RequiredQuantity = 1,
    decimal? SpecialPrice = null);
