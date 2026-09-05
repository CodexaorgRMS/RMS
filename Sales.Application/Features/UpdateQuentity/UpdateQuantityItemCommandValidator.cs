using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;

namespace Sales.Application.Features.UpdateQuentity
{
	public class UpdateQuantityItemCommandValidator : AbstractValidator<UpdateQuantityItemCommand>
	{
		private readonly ISalesDataContext _context;
		public UpdateQuantityItemCommandValidator(ISalesDataContext context)
		{
			_context = context;

			RuleFor(x => x.Quantity)
				.GreaterThan(0).WithMessage("Quantity must be greater than 0")
				.MustAsync(quantityrule)
				.WithMessage("Quantity must be different from the current quantity");

			RuleFor(x => x.orderNumber)
				.NotEmpty()
				.WithMessage("OrderNumber is required.")
				.MustAsync(OrderExists)
				.WithMessage("Order not found.")
				.MustAsync(IsOrderPended)
				.WithMessage("Order is not in a pending state.");

			RuleFor(x => x.orderItemId)
				.NotEmpty()
				.WithMessage("OrderItemId is required.")
				.MustAsync(OrderItemExists)
				.WithMessage("Order item not found.");
		}

		private async Task<bool> quantityrule(UpdateQuantityItemCommand command, int quantity, CancellationToken cancellationToken)
		{
			var orderItem = await _context.OrderItems.FindAsync(new object[] { command.orderItemId }, cancellationToken);
			if (orderItem == null)
			{
				return false; // Order item not found
			}
			return orderItem.Quantity != quantity;
		}

		private async Task<bool> OrderItemExists(Guid orderItemId, CancellationToken cancellationToken)
		{
			return await _context.OrderItems.AnyAsync(oi => oi.OrderItemId == orderItemId, cancellationToken);
		}

		private async Task<bool> OrderExists(string orderNumber, CancellationToken cancellationToken)
		{
			return await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber, cancellationToken);
		}

		private async Task<bool> IsOrderPended(string orderNumber, CancellationToken cancellationToken)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
			return order != null && order.Status == OrderStatus.Pending;
		}
	}
}
