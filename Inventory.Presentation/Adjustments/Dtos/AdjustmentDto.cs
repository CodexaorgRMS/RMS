namespace Inventory.Presentation.Adjustments.Dtos
{
	public class AdjustmentDto
	{
		public Guid AdjustmentId { get; set; }
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public Guid ProductBatchId { get; set; }
		public string Type { get; set; } = string.Empty;
		public string Reason { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal TotalFinancialImpact { get; set; }
		public string? Note { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
