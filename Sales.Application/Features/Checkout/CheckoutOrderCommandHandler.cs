using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout;

[Transactional]
public static class CheckoutOrderCommandHandler
{
	public static async Task<Result> Handle(
		CheckoutOrderCommand command,
		ISalesDataContext context,
		IMessageBus bus,
		CancellationToken cancellationToken)
	{
		// All validation is handled by CheckoutOrderCommandValidator.
		// Fetch the Order along with its OrderItems.
		var order = await context.Orders
			.Include(o => o.Items)
			.FirstAsync(o => o.OrderId == command.orderId, cancellationToken);

		// Update PaidAmount and CustomerId
		order.PaidAmount = command.PaidAmount;
		order.CustomerId = command.CustomerId;

		// Mark as Completed
		order.Status = OrderStatus.Completed;

		// Map OrderItems to SoldItemDto
		var soldItems = order.Items.Select(item =>
			new SoldItemDto(item.ProductId, item.Quantity));

		// Publish the integration event
		await bus.PublishAsync(new OrderCompletedEvent(
			OrderId: order.OrderId,
			CustomerId: command.CustomerId,
			TotalAmount: order.TotalAmount,
			PaidAmount: command.PaidAmount,
			Items: soldItems,
			CompletedAt: DateTime.UtcNow));

		return Result.Ok();
	}
}
