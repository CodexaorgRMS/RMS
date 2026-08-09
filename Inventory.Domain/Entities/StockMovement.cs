using System;

namespace Inventory.Domain.Entities
{
    public class StockMovement
    {
        public Guid MovementId { get; set; }
        public Guid ProductId { get; set; }
		public string Type { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public Guid ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }

        public  Product Product { get; set; } = null!;
    }
}
