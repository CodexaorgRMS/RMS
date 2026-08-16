using FluentResults;
using FluentValidation.Results;
using SharedContracts.Inventory.Commands;
using SharedContracts.Sales.Events;
using SharedKernel.Exeptions;
using Wolverine;

namespace Inventory.Application.Features.Stocks.EventHandlers;

public static class OrderRefundedEventInventoryHandler
{
	public static async Task Handle(
		OrderRefundedEvent @event,
		IMessageBus bus)
	{
		foreach (var item in @event.Items)
		{
			var command = new RestoreStockCommand(
				item.ProductId,
				item.Quantity,
				@event.OrderId);

			var result = await bus.InvokeAsync<Result>(command);

			if (result.IsFailed)
			{
				var failures = result.Errors
					.Select(error => new ValidationFailure("RestoreStock", error.Message))
					.ToList();

				throw new CommandValidationException(failures);
			}
		}
	}
}
