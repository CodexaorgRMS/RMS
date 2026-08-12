namespace Inventory.Application.Features.InventoryItems.Events;

public sealed record InventoryStockmovementEvent(
	Guid InventoryItemId,
	Guid ProductId,
	string Type,
	int Quantity,
	DateTime OccurredAt);