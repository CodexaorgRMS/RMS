namespace Offers.Presentation.Offers.Dtos;

public sealed record OfferDto(
    Guid OfferId,
    string Name,
    string? Description,
    string Type,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    bool IsSmart,
    int Priority,
    DateTime CreatedAt,
    List<OfferTargetDto> Targets);
