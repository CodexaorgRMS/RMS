namespace Sales.Presentation.Dtos
{
	public class OrderItemDto
	{
		public Guid OrderItemId { get; set; }
		public Guid OrderId { get; set; }
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
