namespace SharedContracts.Sales.Events;

public record OrderCompletedEvent(
	Guid OrderId,
	Guid CustomerId,
	decimal TotalAmount,
	decimal PaidAmount,
	DateTime CompletedAt);
