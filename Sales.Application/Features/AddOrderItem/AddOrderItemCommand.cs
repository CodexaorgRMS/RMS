namespace Sales.Application.Features.AddOrderItem
{
	public record AddOrderItemCommand(Guid orderId,Guid ProductId, int Quantity);
}
