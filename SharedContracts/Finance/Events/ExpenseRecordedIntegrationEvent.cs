namespace SharedContracts.Finance.Events;

public sealed record ExpenseRecordedIntegrationEvent(
    Guid ExpenseId,
    decimal Amount,
    string CategoryName,
    string Source,
    DateTime OccurredAt);
