using System;

namespace Inventory.Domain.Entities
{
    public class InventoryItem
    {
        public Guid InventoryItemId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int MinStock { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
