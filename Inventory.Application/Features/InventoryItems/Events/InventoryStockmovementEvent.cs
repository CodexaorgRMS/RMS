using Inventory.Domain.Entities;

namespace Inventory.Application.Features.InventoryItems.Events;

public sealed record InventoryStockmovementEvent(
	Guid InventoryItemId,
	Guid ProductId,
	StockMovementType Type,
	int Quantity,
	DateTime OccurredAt);