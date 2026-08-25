using Offers.Domain.Enums;

namespace Offers.Application.Features.Offers.DTOs;

public sealed record CreateOfferTargetDto(
    OfferTargetType TargetType,
    Guid TargetId,
    int RequiredQuantity = 1,
    decimal? SpecialPrice = null);
