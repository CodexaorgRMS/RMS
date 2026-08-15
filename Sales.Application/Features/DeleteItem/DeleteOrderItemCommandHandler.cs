using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Wolverine.Attributes;

namespace Sales.Application.Features.DeleteItem
{
	[Transactional]
	public static class DeleteOrderItemCommandHandler
	{
		public static async Task<Result> Handle(DeleteOrderItemCommand command, ISalesDataContext context, CancellationToken cancellationToken)
		{
			var order = await context.Orders
				.Include(o => o.Items)
				.FirstOrDefaultAsync(o => o.OrderId == command.orderId, cancellationToken);
		
			var orderItem = order!.Items.FirstOrDefault(oi => oi.OrderItemId == command.orderitemId);

			 context.OrderItems.Remove(orderItem!);


			return Result.Ok();
		}
	}

}
