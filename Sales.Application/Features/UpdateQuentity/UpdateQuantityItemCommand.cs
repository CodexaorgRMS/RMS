namespace Sales.Application.Features.UpdateQuentity
{
	public record UpdateQuantityItemCommand(Guid orderItemId, int Quantity,string orderNumber);
}
