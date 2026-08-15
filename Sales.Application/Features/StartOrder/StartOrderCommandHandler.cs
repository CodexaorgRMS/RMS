using FluentResults;
using Sales.Application.Abstractions;
using Sales.Domain.Entities;
using Wolverine.Attributes;

namespace Sales.Application.Features.StartOrder
{
	[Transactional]
	public static class StartOrderCommandHandler
	{
		public static async Task<Result<string>> Handle(StartOrderCommand command,
			ISalesDataContext context)
		{
		
			var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";

			var order = new Order
			{
				OrderNumber = orderNumber
			};

			await context.Orders.AddAsync(order);

			return Result.Ok(orderNumber);
		}
	}
}
