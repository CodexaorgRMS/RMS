using Customers.Domain.Enums;

namespace Customers.Domain.Entities;

public class CustomerLedger
{
	public Guid LedgerId { get; set; }
	public Guid CustomerId { get; set; }
	public virtual Customer Customer { get; set; } = null!;
	public LedgerType Type { get; set; }
	public decimal Amount { get; set; }
	public string? Reason { get; set; }
	public Guid? ReferenceOrderId { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
