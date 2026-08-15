using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;

namespace Sales.Application.Features.DeleteItem
{
	public class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
	{
		private readonly ISalesDataContext _context;
		public DeleteOrderItemCommandValidator(ISalesDataContext context)
		{
			_context = context;

			RuleFor(x => x.orderId)
				.NotEmpty()
				.WithMessage("OrderId is required.")
				.MustAsync(OrderExists)
				.WithMessage("Order not found.")
				.MustAsync(IsOrderPended)
				.WithMessage("Order is not in a pending state.");

			RuleFor(x => x.orderitemId)
				.NotEmpty()
				.WithMessage("OrderItemId is required.")
				.MustAsync(OrderItemExists)
				.WithMessage("Order item not found.");
		}

		private async Task<bool> OrderItemExists(Guid orderitemId, CancellationToken cancellationToken)
		{
			return await _context.OrderItems.AnyAsync(oi => oi.OrderItemId == orderitemId, cancellationToken);
		}

		private async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
		{
			return await _context.Orders.AnyAsync(o => o.OrderId == orderId, cancellationToken);
		}

		private async Task<bool> IsOrderPended(Guid orderId, CancellationToken cancellationToken)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);
			return order != null && order.Status == Domain.Enums.OrderStatus.Pending;
		}
	}

}
