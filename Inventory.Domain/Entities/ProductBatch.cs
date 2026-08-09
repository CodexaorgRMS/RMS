using System;

namespace Inventory.Domain.Entities
{
    public class ProductBatch
    {
        public Guid BatchId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
