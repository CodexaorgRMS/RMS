using Sales.Domain.Enums;

namespace Sales.Presentation.Dtos
{
	public class OrderDto
	{
		public Guid OrderId { get; set; }
		public string OrderNumber { get; set; } = string.Empty;
		public Guid? CustomerId { get; set; }
		public decimal SubTotal { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal PaidAmount { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public List<OrderItemDto> Items { get; set; } = new();
	}
}
