namespace SharedContracts.Customers.Events;

public record CustomerPaymentReceivedEvent(
	Guid CustomerId,
	decimal PaidAmount,
	DateTime ReceivedAt);


