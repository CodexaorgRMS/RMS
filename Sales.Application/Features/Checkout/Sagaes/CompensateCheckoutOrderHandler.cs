using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Saga;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout.Sagaes;

/// <summary>
/// Compensation: reverts a finalized order back to Pending status.
/// Called by the saga orchestrator when a downstream step fails
/// after the order has already been marked as Completed.
/// </summary>
public static class CompensateCheckoutOrderHandler
{
	[Transactional]
	public static async Task Handle(
		CompensateCheckoutOrder command,
		ISalesDataContext context,
		CancellationToken cancellationToken)
	{
		var order = await context.Orders
			.FirstOrDefaultAsync(o => o.OrderId == command.OrderId, cancellationToken);

		if (order is not null)
		{
			order.Status = OrderStatus.Pending;
			order.PaidAmount = 0;
			order.CustomerId = null;
		}
	}
}
