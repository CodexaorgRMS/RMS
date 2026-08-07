using Domain.Enums;

namespace Domain.Entities
{
	public class OrderPayment
	{
		public Guid Id { get; set; }

		public Guid SaleId { get; set; }

		public PaymentMethod Method { get; set; }

		public decimal Amount { get; set; }

		// Navigation Property
		public Sale Sale { get; set; } = null!;
	}
}
