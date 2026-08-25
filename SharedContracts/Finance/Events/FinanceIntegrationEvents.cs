namespace SharedContracts.Finance.Events;

public sealed record CashShiftClosedWithVarianceIntegrationEvent(
    Guid ShiftId,
    Guid CashierId,
    decimal ExpectedCash,
    decimal ActualCash,
    decimal Variance,
    DateTime OccurredAt);

public sealed record ExpenseRecordedIntegrationEvent(
    Guid ExpenseId,
    decimal Amount,
    string CategoryName,
    string Source,
    DateTime OccurredAt);

public sealed record ObligationSettledIntegrationEvent(
    Guid ObligationId,
    decimal AmountPaid,
    decimal RemainingAmount,
    string Source,
    DateTime OccurredAt);
