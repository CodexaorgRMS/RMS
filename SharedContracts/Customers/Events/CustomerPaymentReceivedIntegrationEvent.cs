namespace SharedContracts.Customers.Events;

public sealed record CustomerPaymentReceivedIntegrationEvent(
    Guid CustomerId,
    decimal AmountPaid,
    string PaymentMethod,
    Guid CashierId,
    DateTime OccurredAt);
