using FluentResults;
using FluentValidation.Results;
using SharedContracts.Inventory.Commands;
using SharedContracts.Sales.Events;
using SharedKernel.Exeptions;
using Wolverine;

namespace Inventory.Application.Features.Stocks.EventHandlers;

public static class OrderCompletedEventInventoryHandler
{
	public static async Task Handle(
		OrderCompletedEvent @event,
		IMessageBus bus)
	{
		foreach (var item in @event.Items)
		{
			var command = new DeductStockCommand(
				item.ProductId,
				item.Quantity,
				@event.OrderId);

			var result = await bus.InvokeAsync<Result>(command);

			if (result.IsFailed)
			{
				var failures = result.Errors
					.Select(error => new ValidationFailure("DeductStock", error.Message))
					.ToList();

				throw new CommandValidationException(failures);
			}
		}
	}
}
