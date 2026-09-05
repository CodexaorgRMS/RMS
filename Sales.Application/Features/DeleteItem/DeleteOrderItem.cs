namespace Sales.Application.Features.DeleteItem
{
	public record DeleteOrderItemCommand(string orderNumber,Guid orderitemId);

}
