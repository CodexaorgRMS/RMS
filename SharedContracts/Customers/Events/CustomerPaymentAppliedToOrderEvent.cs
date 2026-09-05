namespace SharedContracts.Customers.Events;

public record CustomerPaymentAppliedToOrderEvent(
	string OrderNumber,
	decimal PaidAmount);
