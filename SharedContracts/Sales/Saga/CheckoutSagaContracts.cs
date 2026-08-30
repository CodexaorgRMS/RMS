using SharedContracts.Sales.Events;

namespace SharedContracts.Sales.Saga;

// ── Saga State ─────────────────────────────────────────────────────────────────

/// <summary>
/// Represents the persistent state of a Checkout Saga instance.
/// Tracks which steps have completed and stores data needed for compensation.
/// </summary>
public class CheckoutSagaState
{
	public Guid SagaId { get; set; } = Guid.NewGuid();
	public Guid OrderId { get; set; }
	public decimal PaidAmount { get; set; }
	public Guid? CustomerId { get; set; }
	public CheckoutSagaStatus Status { get; set; } = CheckoutSagaStatus.NotStarted;

	/// <summary>Items that were successfully deducted from inventory (for rollback).</summary>
	public List<SoldItemDto> DeductedItems { get; set; } = new();

	/// <summary>All items in the order (the full set to process).</summary>
	public List<SoldItemDto> AllItems { get; set; } = new();

	public decimal SubTotal { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal TotalAmount { get; set; }

	public string? FailureReason { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? CompletedAt { get; set; }
}

public enum CheckoutSagaStatus
{
	NotStarted,
	StockValidated,
	StockDeducted,
	OrderCompleted,
	FinanceRecorded,
	Completed,
	Compensating,
	Failed
}

// ── Saga Step Commands (internal orchestration) ────────────────────────────────

/// <summary>Start the saga: validate stock availability for all items.</summary>
public record StartCheckoutSaga(Guid OrderId, decimal PaidAmount, Guid? CustomerId);

/// <summary>Step 1b: Validate stock availability for all items via the Inventory module.</summary>
public record ValidateCheckoutStock(Guid SagaId, List<SoldItemDto> Items);

/// <summary>Step 2: Deduct stock from inventory for all items atomically.</summary>
/// 

public record DeductStockForCheckout(Guid SagaId, Guid OrderId, List<SoldItemDto> Items);

/// <summary>Step 3: Finalize the order status in the Sales module.</summary>
public record FinalizeCheckoutOrder(
	Guid SagaId,
	Guid OrderId,
	decimal PaidAmount,
	Guid? CustomerId,
	decimal SubTotal,
	decimal DiscountAmount,
	decimal TotalAmount,
	List<SoldItemDto> Items);

/// <summary>Step 4: Publish the completed event for Finance and other downstream modules.</summary>
public record PublishCheckoutCompleted(
	Guid SagaId,
	Guid OrderId,
	Guid? CustomerId,
	decimal TotalAmount,
	decimal PaidAmount,
	List<SoldItemDto> Items);

// ── Compensation Commands ──────────────────────────────────────────────────────

/// <summary>Compensate: restore all previously deducted stock.</summary>
public record CompensateCheckoutStock(Guid SagaId, Guid OrderId, List<SoldItemDto> DeductedItems);

/// <summary>Compensate: revert the order status back to Pending.</summary>
public record CompensateCheckoutOrder(Guid SagaId, Guid OrderId);

// ── Saga Step Responses ────────────────────────────────────────────────────────

public record CheckoutStockValidated(Guid SagaId, bool IsValid, string? ErrorMessage = null);
public record CheckoutStockDeducted(Guid SagaId, bool IsSuccess, string? ErrorMessage = null);
public record CheckoutOrderFinalized(Guid SagaId, bool IsSuccess, string? ErrorMessage = null);
public record CheckoutCompletedPublished(Guid SagaId, bool IsSuccess, string? ErrorMessage = null);
public record CheckoutSagaCompleted(Guid SagaId, Guid OrderId, bool IsSuccess, string? ErrorMessage = null);

