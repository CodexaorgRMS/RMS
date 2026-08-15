namespace Customers.Domain.Entities;

public class Customer
{
	public Guid CustomerId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public decimal TotalDebt { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public virtual ICollection<CustomerLedger> Ledgers { get; set; } = new List<CustomerLedger>();
}
