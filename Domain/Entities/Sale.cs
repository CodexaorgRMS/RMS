using Domain.Enums;

namespace Domain.Entities
{

	public class Sale
	{
		public Guid Id { get; set; }

		public string OrderNumber { get; set; } = string.Empty;

		public Guid CashierId { get; set; }

		public OrderStatus Status { get; set; }

		public decimal TotalAmount { get; set; }

		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
	}
}
