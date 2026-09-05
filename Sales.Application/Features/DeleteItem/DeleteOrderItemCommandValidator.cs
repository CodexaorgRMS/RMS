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

			RuleFor(x => x.orderNumber)
				.NotEmpty()
				.WithMessage("OrderNumber is required.")
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

		private async Task<bool> OrderExists(string orderNumber, CancellationToken cancellationToken)
		{
			return await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber, cancellationToken);
		}

		private async Task<bool> IsOrderPended(string orderNumber, CancellationToken cancellationToken)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
			return order != null && order.Status == Domain.Enums.OrderStatus.Pending;
		}
	}

}
