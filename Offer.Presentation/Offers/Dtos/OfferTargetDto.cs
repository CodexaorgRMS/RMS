namespace Offers.Presentation.Offers.Dtos;

public sealed record OfferTargetDto(
    Guid OfferTargetId,
    Guid OfferId,
    string TargetType,
    Guid TargetId,
    int RequiredQuantity,
    decimal? SpecialPrice);
