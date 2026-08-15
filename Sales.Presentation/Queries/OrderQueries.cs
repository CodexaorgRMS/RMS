using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Presentation.Dtos;
using SharedPresentation.GraphQL;

namespace Sales.Presentation.Queries
{
	[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
	public class OrderQueries
	{
		/// <summary>
		/// Gets a paged, filterable, and sortable list of all orders including their items.
		/// </summary>
		[UsePaging(IncludeTotalCount = true)]
		[UseFiltering]
		[UseSorting]
		public IQueryable<OrderDto> GetOrders([Service] ISalesDataContext context)
		{
			return context.Orders
						.Include(o => o.Items)
				.AsNoTracking()
				.Select(o => new OrderDto
				{
					OrderId = o.OrderId,
					OrderNumber = o.OrderNumber,
					CustomerId = o.CustomerId,
					SubTotal = o.SubTotal,
					DiscountAmount = o.DiscountAmount,
					TotalAmount = o.TotalAmount,
					PaidAmount = o.PaidAmount,
					Status = o.Status.ToString(),
					CreatedAt = o.CreatedAt,
					Items = o.Items.Select(i => new OrderItemDto
					{
						OrderItemId = i.OrderItemId,
						OrderId = i.OrderId,
						ProductId = i.ProductId,
						ProductName = i.ProductName,
						UnitPrice = i.UnitPrice,
						Quantity = i.Quantity,
						TotalPrice = i.TotalPrice
					}).ToList()
				});
		}

		/// <summary>
		/// Gets a single order by its ID including its order items.
		/// </summary>
		[UseFirstOrDefault]
		public IQueryable<OrderDto> GetOrderById(
			Guid orderId,
			[Service] ISalesDataContext context,
			CancellationToken cancellationToken)
		{
			return  context.Orders
				.Include(o => o.Items)
				.AsNoTracking()
				.Where(o => o.OrderId == orderId)
				.Select(o => new OrderDto
				{
					OrderId = o.OrderId,
					OrderNumber = o.OrderNumber,
					CustomerId = o.CustomerId,
					SubTotal = o.SubTotal,
					DiscountAmount = o.DiscountAmount,
					TotalAmount = o.TotalAmount,
					PaidAmount = o.PaidAmount,
					Status = o.Status.ToString(),
					CreatedAt = o.CreatedAt,
					Items = o.Items.Select(i => new OrderItemDto
					{
						OrderItemId = i.OrderItemId,
						OrderId = i.OrderId,
						ProductId = i.ProductId,
						ProductName = i.ProductName,
						UnitPrice = i.UnitPrice,
						Quantity = i.Quantity,
						TotalPrice = i.TotalPrice
					}).ToList()
				});
		}

	
	}
}
