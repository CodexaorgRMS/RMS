namespace Domain.Entities
{
	public class SaleItem
	{
		public Guid Id { get; set; }

		public Guid ProductId { get; set; }

		public Guid SaleId { get; set; }

		public string ProductName { get; set; } = string.Empty;
		public string Barcode { get; set; } = string.Empty;

		public decimal Quantity { get; set; }

		public decimal UnitPrice { get; set; }
		public decimal SubTotal { get; set; }

		public bool IsRefunded { get; set; } = false;

		// Navigation Property
		public Sale Sale { get; set; } = null!;
	}
}
