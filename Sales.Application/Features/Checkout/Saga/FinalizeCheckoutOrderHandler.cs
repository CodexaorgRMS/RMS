using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Saga;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout.Saga;

/// <summary>
/// Saga Step 3: Finalizes the order by updating totals and marking it as Completed.
/// Runs in its own [Transactional] boundary so it can be independently compensated.
/// </summary>
public static class FinalizeCheckoutOrderHandler
{
	[Transactional]
	public static async Task<CheckoutOrderFinalized> Handle(
		FinalizeCheckoutOrder command,
		ISalesDataContext context,
		CancellationToken cancellationToken)
	{
		var order = await context.Orders
			.Include(o => o.Items)
			.FirstOrDefaultAsync(o => o.OrderId == command.OrderId, cancellationToken);

		if (order is null)
		{
			return new CheckoutOrderFinalized(command.SagaId, false,
				$"Order '{command.OrderId}' not found during finalization.");
		}

		order.SubTotal = command.SubTotal;
		order.DiscountAmount = command.DiscountAmount;
		order.TotalAmount = command.TotalAmount;
		order.PaidAmount = command.PaidAmount;
		order.CustomerId = command.CustomerId;
		order.Status = OrderStatus.Completed;

		return new CheckoutOrderFinalized(command.SagaId, true);
	}
}
