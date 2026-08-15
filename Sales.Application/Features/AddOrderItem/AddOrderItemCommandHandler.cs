using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Entities;
using SharedContracts.Inventory.Interfaces;
using Wolverine.Attributes;

namespace Sales.Application.Features.AddOrderItem
{
	[Transactional(typeof(ISalesDataContext))]
	public static class AddOrderItemCommandHandler
	{
		public static async Task<Result<Guid>> Handle(AddOrderItemCommand command,
			ISalesDataContext context,
			IProductService productService)
		{
			var order = await context.Orders.FindAsync(command.orderId);
			if (order is null)
			{
				return Result.Fail<Guid>("Order not found.");
			}
		
			var product = await productService.GetProductByIdAsync(command.ProductId);
			if (product is null)
			{
				return Result.Fail<Guid>("Product not found.");
			}

			var existingOrderItem = await context.OrderItems
						 .FirstOrDefaultAsync(x =>
							 x.OrderId == command.orderId &&
							 x.ProductId == command.ProductId);
			var itemId = Guid.Empty;

			if (existingOrderItem is not null)
			{
				existingOrderItem.Quantity += command.Quantity;

				existingOrderItem.TotalPrice =
					existingOrderItem.Quantity * existingOrderItem.UnitPrice;

				itemId = existingOrderItem.OrderItemId;
			}
			else
			{
				var orderItem = new OrderItem
				{
					OrderId = command.orderId,
					ProductId = command.ProductId,
					ProductName = product.Name,
					Quantity = command.Quantity,
					UnitPrice = product.SellingPrice,
					TotalPrice = product.SellingPrice * command.Quantity
				};
				itemId = orderItem.OrderItemId;

				await context.OrderItems.AddAsync(orderItem);
			}

			return Result.Ok(itemId);
		}
	}
}
