using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Sales.Application.Features.Refund;

[Transactional]
public static class RefundOrderCommandHandler
{
	public static async Task<Result> Handle(
		RefundOrderCommand command,
		ISalesDataContext context,
		IMessageBus bus,
		CancellationToken cancellationToken)
	{
		// All validation is handled by RefundOrderCommandValidator.
		var order = await context.Orders
			.Include(o => o.Items)
			.FirstAsync(o => o.OrderId == command.orderId, cancellationToken);

		// Mark as Refunded
		order.Status = OrderStatus.Refunded;

		// Map OrderItems to RefundedItemDto
		var refundedItems = order.Items.Select(item =>
			new RefundedItemDto(item.ProductId, item.Quantity));

		// Publish the integration event
		await bus.PublishAsync(new OrderRefundedEvent(
			OrderId: order.OrderId,
			CustomerId: order.CustomerId,
			TotalAmount: order.TotalAmount,
			PaidAmount: order.PaidAmount,
			Items: refundedItems,
			RefundedAt: DateTime.UtcNow));

		return Result.Ok();
	}
}
