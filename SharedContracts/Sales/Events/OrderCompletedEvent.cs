namespace SharedContracts.Sales.Events;

public record OrderCompletedEvent(
	Guid OrderId,
	Guid? CustomerId,
	decimal TotalAmount,
	decimal PaidAmount,
	IEnumerable<SoldItemDto> Items,
	DateTime CompletedAt);

public record SoldItemDto(Guid ProductId, int Quantity);
