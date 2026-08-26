namespace SharedContracts.Offers.Events;

public sealed record OfferDeactivatedIntegrationEvent(
    Guid OfferId,
    DateTime OccurredAt);
