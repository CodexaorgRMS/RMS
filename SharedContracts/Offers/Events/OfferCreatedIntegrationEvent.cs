namespace SharedContracts.Offers.Events;

public sealed record OfferCreatedIntegrationEvent(
    Guid OfferId,
    string Name,
    string Type,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    DateTime OccurredAt);
