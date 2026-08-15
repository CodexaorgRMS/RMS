using Sales.Domain.Enums;

namespace Sales.Domain.Entities
{
	public class Order
	{
		public Guid OrderId { get; set; }

		public string OrderNumber { get; set; } = string.Empty;

		// ربط الفاتورة بعميل في حالة الشكك (Nullable لأن أغلب زبائن السوبر ماركت طيارى)
		public Guid? CustomerId { get; set; }

		// إجمالي الفاتورة قبل أي خصومات
		public decimal SubTotal { get; set; }

		// إجمالي الخصومات (القادمة من موديول العروض)
		public decimal DiscountAmount { get; set; }

		// المبلغ المطلوب دفعه (SubTotal - DiscountAmount)
		public decimal TotalAmount { get; set; }

		// المبلغ الذي دفعه الزبون فعلياً الكاشير (لضبط حسابات الشكك)
		public decimal PaidAmount { get; set; }

		public OrderStatus Status { get; set; } = OrderStatus.Pending;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Property
		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
	}
}
