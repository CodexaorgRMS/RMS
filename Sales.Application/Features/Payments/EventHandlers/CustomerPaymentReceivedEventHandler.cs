using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Customers.Events;

namespace Sales.Application.Features.Payments.EventHandlers;

public static class CustomerPaymentReceivedEventHandler
{
	[Wolverine.Attributes.Transactional]
	public static async Task Handle(
		CustomerPaymentAppliedToOrderEvent @event,
		ISalesDataContext context,
		CancellationToken cancellationToken)
	{

		var order = await context.Orders
			.FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber, cancellationToken);

		if (order is null)
		{
			return; // Order not found, maybe invalid order number
		}

		if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Refunded || order.Status == OrderStatus.Cancelled)
		{
			return; // Order is already closed
		}

		// Update the paid amount
		order.PaidAmount += @event.PaidAmount;

		// If fully paid, change status to Completed
		if (order.PaidAmount >= order.TotalAmount)
		{
			order.Status = OrderStatus.Completed;
		}
		else
		{
			order.Status = OrderStatus.PartiallyPaid;
		}

		// SaveChangesAsync will be called automatically by Wolverine's Outbox middleware
	}
}
