using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;

namespace Sales.Application.Features.Refund;

public class RefundOrderCommandValidator : AbstractValidator<RefundOrderCommand>
{
	private readonly ISalesDataContext _context;

	public RefundOrderCommandValidator(ISalesDataContext context)
	{
		_context = context;

		RuleFor(x => x.orderId)
			.NotEmpty()
			.WithMessage("Order ID is required.")
			.MustAsync(OrderExists)
			.WithMessage("Order not found.")
			.MustAsync(OrderIsCompleted)
			.WithMessage("Only completed orders can be refunded.");
	}

	private async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
	{
		return await _context.Orders.AnyAsync(o => o.OrderId == orderId, cancellationToken);
	}

	private async Task<bool> OrderIsCompleted(Guid orderId, CancellationToken cancellationToken)
	{
		return await _context.Orders.AnyAsync(
			o => o.OrderId == orderId && o.Status == OrderStatus.Completed, cancellationToken);
	}
}
