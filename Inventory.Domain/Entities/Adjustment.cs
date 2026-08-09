using System;

namespace Inventory.Domain.Entities
{
    public class Adjustment
    {
        public Guid AdjustmentId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
		public string Type { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}
