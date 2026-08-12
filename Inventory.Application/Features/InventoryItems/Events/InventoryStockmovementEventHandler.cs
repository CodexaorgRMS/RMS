using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Events
{
	[Transactional]
	public static class InventoryStockmovementEventHandler
	{
		public static async Task Handle(InventoryStockmovementEvent @event, IInventoryDataContext context, CancellationToken cancellationToken)
		{
			var movement = new StockMovement
			{
				MovementId = Guid.NewGuid(),
				ProductId = @event.ProductId,
				Type = @event.Type,
				Quantity = @event.Quantity,
				ReferenceId = @event.InventoryItemId,
				CreatedAt = @event.OccurredAt
			};

			await context.StockMovements.AddAsync(
		  movement,
		  cancellationToken);
		}
	}
}
