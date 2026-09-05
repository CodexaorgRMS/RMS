namespace SharedContracts.Finance.Events;

public sealed record ObligationSettledIntegrationEvent(
    Guid ObligationId,
    decimal AmountPaid,
    decimal RemainingAmount,
    string Source,
    DateTime OccurredAt);
