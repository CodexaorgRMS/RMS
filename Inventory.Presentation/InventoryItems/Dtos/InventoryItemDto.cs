namespace Inventory.Presentation.InventoryItems.Dtos;

public sealed class InventoryItemDto
{
    public Guid InventoryItemId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public int MinStock { get; set; }

    public DateTime UpdatedAt { get; set; }
}