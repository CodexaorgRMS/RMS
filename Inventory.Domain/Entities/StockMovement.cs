using System;

namespace Inventory.Domain.Entities
{
	public enum StockMovementType
	{
		In,
		Out,
		Adjustment,
		Outbound_Sale
	}
	public class StockMovement
	{
		public Guid MovementId { get; set; }

		public Guid ProductId { get; set; }
		public virtual Product Product { get; set; } = null!;
		public Guid ProductBatchId { get; set; }
		public virtual ProductBatch ProductBatch { get; set; } = null!;

		public StockMovementType Type { get; set; } 
		public int Quantity { get; set; }

		public Guid? ReferenceId { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}