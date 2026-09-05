namespace SharedContracts.Finance.Events;

public sealed record CashShiftClosedWithVarianceIntegrationEvent(
    Guid ShiftId,
    Guid CashierId,
    decimal ExpectedCash,
    decimal ActualCash,
    decimal Variance,
    DateTime OccurredAt);
