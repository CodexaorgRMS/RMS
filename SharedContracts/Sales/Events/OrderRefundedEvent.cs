namespace SharedContracts.Sales.Events;

public record OrderRefundedEvent(
	Guid OrderId,
	Guid? CustomerId,
	decimal TotalAmount,
	decimal PaidAmount,
	IEnumerable<RefundedItemDto> Items,
	DateTime RefundedAt);

public record RefundedItemDto(Guid ProductId, int Quantity);
