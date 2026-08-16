using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;

namespace Sales.Application.Features.Checkout;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
	private readonly ISalesDataContext _context;

	public CheckoutOrderCommandValidator(ISalesDataContext context)
	{
		_context = context;

		RuleFor(x => x.orderId)
			.NotEmpty()
			.WithMessage("Order ID is required.")
			.MustAsync(OrderExists)
			.WithMessage("Order not found.")
			.MustAsync(OrderIsPending)
			.WithMessage("Order is not in Pending status.")
			.MustAsync(OrderHasItems)
			.WithMessage("Cannot checkout an order with no items.");

		RuleFor(x => x.PaidAmount)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Paid amount must be greater than or equal to zero.");

		RuleFor(x => x)
			.MustAsync(CreditRuleSatisfied)
			.WithMessage("A Customer ID is required when the paid amount is less than the total (credit sale).")
			.WithName("CustomerId");
	}

	private async Task<bool> OrderExists(Guid orderId, CancellationToken cancellationToken)
	{
		return await _context.Orders.AnyAsync(o => o.OrderId == orderId, cancellationToken);
	}

	private async Task<bool> OrderIsPending(Guid orderId, CancellationToken cancellationToken)
	{
		return await _context.Orders.AnyAsync(
			o => o.OrderId == orderId && o.Status == OrderStatus.Pending, cancellationToken);
	}

	private async Task<bool> OrderHasItems(Guid orderId, CancellationToken cancellationToken)
	{
		return await _context.OrderItems.AnyAsync(i => i.OrderId == orderId, cancellationToken);
	}

	private async Task<bool> CreditRuleSatisfied(CheckoutOrderCommand command, CancellationToken cancellationToken)
	{
		var order = await _context.Orders
			.AsNoTracking()
			.FirstOrDefaultAsync(o => o.OrderId == command.orderId, cancellationToken);

		if (order is null)
			return true; // Already caught by OrderExists rule

		if (command.PaidAmount < order.TotalAmount && command.CustomerId is null)
			return false;

		return true;
	}
}
