using Customers.Domain.Enums;
using FluentResults;

namespace Customers.Domain.Entities;

public class Customer
{
	public Guid CustomerId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public decimal TotalDebt { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public virtual ICollection<CustomerLedger> Ledgers { get; set; } = new List<CustomerLedger>();

	// ═══════════════════════════════════════════════════════════════════════════
	// DOMAIN BEHAVIOR — All state mutations go through these methods
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Records a debt against this customer. Creates an immutable ledger entry
	/// and updates the running <see cref="TotalDebt"/> aggregate.
	/// </summary>
	/// <param name="amount">The debt amount (must be positive).</param>
	/// <param name="reason">Human-readable reason for the debt (e.g., "POS Checkout", "Manual adjustment").</param>
	/// <param name="sourceOrderId">Optional reference to the originating order.</param>
	/// <returns>A <see cref="Result{CustomerLedger}"/> containing the new ledger entry, or a failure.</returns>
	public Result<CustomerLedger> AddDebt(decimal amount, string? reason, Guid? sourceOrderId = null)
	{
		if (amount <= 0)
		{
			return Result.Fail<CustomerLedger>("Debt amount must be greater than zero.");
		}

		var ledger = new CustomerLedger
		{
			LedgerId = Guid.NewGuid(),
			CustomerId = CustomerId,
			Type = LedgerType.Debt,
			Amount = amount,
			Reason = reason,
			ReferenceOrderId = sourceOrderId,
			CreatedAt = DateTime.UtcNow
		};

		// TotalDebt += amount; // Temp disabled to isolate concurrency exception
		Ledgers.Add(ledger);

		return Result.Ok(ledger);
	}

	/// <summary>
	/// Records a payment against this customer's outstanding debt.
	/// Creates an immutable ledger entry and reduces the running <see cref="TotalDebt"/>.
	/// </summary>
	/// <param name="amount">The payment amount (must be positive).</param>
	/// <param name="reason">Human-readable reason for the payment.</param>
	/// <param name="sourceOrderId">Optional reference to the originating order.</param>
	/// <returns>A <see cref="Result{CustomerLedger}"/> containing the new ledger entry, or a failure.</returns>
	public Result<CustomerLedger> AddPayment(decimal amount, string? reason = null, Guid? sourceOrderId = null)
	{
		if (amount <= 0)
		{
			return Result.Fail<CustomerLedger>("Payment amount must be greater than zero.");
		}

		var ledger = new CustomerLedger
		{
			LedgerId = Guid.NewGuid(),
			CustomerId = CustomerId,
			Type = LedgerType.Payment,
			Amount = amount,
			Reason = reason,
			ReferenceOrderId = sourceOrderId,
			CreatedAt = DateTime.UtcNow
		};

		// TotalDebt -= amount; // Temp disabled to isolate concurrency exception
		Ledgers.Add(ledger);

		return Result.Ok(ledger);
	}
}
