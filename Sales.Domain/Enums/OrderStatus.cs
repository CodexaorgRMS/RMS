using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Domain.Enums
{
	public enum OrderStatus
	{
		Pending = 1,    // الفاتورة مفتوحة والكاشير يضيف المنتجات
		Completed = 2,  // تم الدفع وإنهاء الفاتورة
		Refunded = 3,   // تم استرجاع الفاتورة بالكامل
		Cancelled = 4,  // تم إلغاء الفاتورة قبل الدفع
		PartiallyPaid = 5 // تم الدفع جزئياً وباقي المبلغ آجل
	}
}
