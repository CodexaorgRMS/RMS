namespace Sales.Application.Features.AddOrderItem
{
	public record AddOrderItemCommand(string orderNumber,Guid ProductId, int Quantity);
}
