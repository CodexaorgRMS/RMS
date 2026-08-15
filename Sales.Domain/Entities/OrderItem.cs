namespace Sales.Domain.Entities
{

    public class OrderItem
	{
		public Guid OrderItemId { get; set; }

		public Guid OrderId { get; set; }
		public virtual Order Order { get; set; } = null!;

		// معرف المنتج (كمرجع فقط، لأن التفاصيل الحقيقية في موديول المخازن)
		public Guid ProductId { get; set; }

		// نحفظ اسم المنتج وقت البيع، لأنه لو تغير اسمه في المخزن مستقبلاً، يجب أن تظل الفاتورة القديمة كما هي
		public string ProductName { get; set; } = string.Empty;

		// السعر للقطعة الواحدة وقت البيع
		public decimal UnitPrice { get; set; }

		public int Quantity { get; set; }

		// إجمالي هذا السطر (UnitPrice * Quantity)
		public decimal TotalPrice { get; set; }
	}
}
