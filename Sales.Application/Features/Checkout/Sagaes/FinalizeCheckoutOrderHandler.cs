using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Saga;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout.Sagaes;

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
		// Execute an atomic update to prevent double-checkout race conditions.
		// If another concurrent request already marked it as Completed, updatedRows will be 0.
		int updatedRows = await context.Orders
			.Where(o => o.OrderId == command.OrderId && o.Status == OrderStatus.Pending)
			.ExecuteUpdateAsync(s => s
				.SetProperty(p => p.SubTotal, command.SubTotal)
				.SetProperty(p => p.DiscountAmount, command.DiscountAmount)
				.SetProperty(p => p.TotalAmount, command.TotalAmount)
				.SetProperty(p => p.PaidAmount, command.PaidAmount)
				.SetProperty(p => p.CustomerId, command.CustomerId)
				.SetProperty(p => p.Status, command.PaidAmount < command.TotalAmount ? OrderStatus.PartiallyPaid : OrderStatus.Completed),
				cancellationToken);

		if (updatedRows == 0)
		{
			// The order is missing or was already processed by a concurrent request.
			return new CheckoutOrderFinalized(command.SagaId, false,
				$"Order '{command.OrderId}' not found or is no longer in Pending status. This may be due to a concurrent checkout.");
		}

		return new CheckoutOrderFinalized(command.SagaId, true);
	}
}
