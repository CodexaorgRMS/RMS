namespace Sales.Application.Features.DeleteItem
{
	public record DeleteOrderItemCommand(Guid orderId,Guid orderitemId);

}
