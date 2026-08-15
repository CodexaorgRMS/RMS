using FluentResults;
using Sales.Application.Abstractions;
using Wolverine.Attributes;

namespace Sales.Application.Features.UpdateQuentity
{
	[Transactional]
	public static class UpdateQuantityItemCommandHandler
	{
		public static async Task<Result> Handle(UpdateQuantityItemCommand command,
			ISalesDataContext context)
		{
			var orderItem = await context.OrderItems.FindAsync(command.orderItemId);
			

			orderItem!.Quantity = command.Quantity;
			orderItem.TotalPrice = orderItem.UnitPrice * command.Quantity;

			return Result.Ok();
		}
	}
}
