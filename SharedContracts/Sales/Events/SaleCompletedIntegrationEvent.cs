namespace SharedContracts.Sales.Events;

public sealed record SaleCompletedIntegrationEvent(
    Guid SaleId,
    Guid? CustomerId,
    Guid CashierId,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal CashAmount,
    decimal CardAmount,
    decimal DueAmount,
    DateTime OccurredAt);
