using Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.Entities
{
	public class ProductBatch
	{
		public Guid BatchId { get; set; }
		public Guid ProductId { get; set; }
		public decimal CostPrice { get; set; }

		public int InitialQuantity { get; set; }
		public int CurrentQuantity { get; set; }

		public DateTime ExpiryDate { get; set; }
		public DateTime CreatedAt { get; set; }

        public BatchStatus Status { get; set; } = BatchStatus.Active;
        public string? HoldReason { get; set; }
        public DateTime? StatusChangedAt { get; set; }

        [Timestamp]
		public byte[] RowVersion { get; set; } = null!;

		public virtual Product Product { get; set; } = null!;

		public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
		public virtual ICollection<Adjustment> Adjustments { get; set; } = new List<Adjustment>();
	}
}