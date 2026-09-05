using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Events;
using SharedContracts.Sales.Saga;
using Wolverine;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout.Sagaes;

/// <summary>
/// Wolverine Stateful Saga for the POS Checkout flow.
///
/// This class inherits from <see cref="Wolverine.Saga"/> and uses Wolverine's
/// convention-based routing:
///   • <c>Starts</c> methods initiate a new saga instance.
///   • <c>Handle</c>  methods process subsequent asynchronous events.
///   • <c>MarkCompleted()</c> signals the saga is finished and can be deleted.
///
/// Saga state (order details, deducted items, totals) is persisted directly
/// as properties on this class. Wolverine automatically saves/loads the state
/// between handler invocations.
///
/// Flow:
///   1. StartCheckoutSaga          → Load order, validate, emit ValidateCheckoutStock
///   1b. CheckoutStockValidated    → Validate result, prepare discount items, emit CalculateCheckoutDiscount
///   1c. CheckoutDiscountCalculated→ Apply discount, validate payment, emit DeductStockForCheckout
///   2.  CheckoutStockDeducted     → Confirm deduction, emit FinalizeCheckoutOrder
///   3.  CheckoutOrderFinalized    → Confirm finalization, emit PublishCheckoutCompleted
///   4.  CheckoutCompletedPublished→ Mark saga completed
///
/// Compensation (on failure at any step):
///   - CompensateCheckoutStock → Restore all previously deducted items
///   - CompensateCheckoutOrder → Revert order status to Pending
/// </summary>
public class CheckoutSagaOrchestrator : Saga
{
	// ═══════════════════════════════════════════════════════════════════════════
	// SAGA IDENTITY — Wolverine uses this for message correlation
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Unique identifier for this saga instance. Wolverine automatically
	/// correlates incoming messages to the correct saga using this property.
	/// </summary>
	public Guid Id { get; set; }

	// ═══════════════════════════════════════════════════════════════════════════
	// SAGA STATE — Persisted between handler invocations (merged from CheckoutSagaState)
	// ═══════════════════════════════════════════════════════════════════════════

	public string OrderNumber { get; set; } = string.Empty;
	public Guid OrderId { get; set; }
	public decimal PaidAmount { get; set; }
	public Guid? CustomerId { get; set; }
	public CheckoutSagaStatus Status { get; set; } = CheckoutSagaStatus.NotStarted;

	/// <summary>Items that were successfully deducted from inventory (for rollback).</summary>
	public List<SoldItemDto> DeductedItems { get; set; } = new();

	/// <summary>All items in the order (the full set to process).</summary>
	public List<SoldItemDto> AllItems { get; set; } = new();

	/// <summary>Enriched items with category IDs from the stock validation step.</summary>
	public List<EnrichedItemDto> EnrichedItems { get; set; } = new();

	/// <summary>Order item details needed for discount calculation (carried across steps).</summary>
	public List<CheckoutDiscountItemDto> DiscountItems { get; set; } = new();

	public decimal SubTotal { get; set; }
	public decimal DiscountAmount { get; set; }
	public decimal TotalAmount { get; set; }

	public string? FailureReason { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? CompletedAt { get; set; }

	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 1 — START the Saga: Load order, validate, emit first command
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Initiates a new saga instance. Loads the order from the database,
	/// performs initial validation, and emits <see cref="ValidateCheckoutStock"/>
	/// as the first cascading message.
	/// </summary>
	/// <returns>
	/// A cascading message (<see cref="ValidateCheckoutStock"/>) on success,
	/// or a <see cref="CheckoutSagaCompleted"/> failure response (with MarkCompleted).
	/// </returns>
	[Transactional]
	public async Task<object> Starts(
		StartCheckoutSaga command,
		ISalesDataContext salesContext,
		CancellationToken cancellationToken)
	{
		// ── Hydrate saga state from the initiating command ─────────────────
		Id = command.SagaId;
		OrderNumber = command.OrderNumber;
		PaidAmount = command.PaidAmount;
		CustomerId = command.CustomerId;

		// ── Load the order ─────────────────────────────────────────────────
		var order = await salesContext.Orders
			.Include(o => o.Items)
			.FirstOrDefaultAsync(o => o.OrderNumber == command.OrderNumber, cancellationToken);

		if (order is null)
		{
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, command.OrderNumber, false,
				$"Order '{command.OrderNumber}' not found.");
		}

		OrderId = order.OrderId;

		if (order.Status != OrderStatus.Pending)
		{
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, command.OrderNumber, false,
				$"Order '{command.OrderNumber}' is not in Pending status.");
		}

		if (!order.Items.Any())
		{
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, command.OrderNumber, false,
				"Cannot checkout an order with no items.");
		}

		// ── Prepare item lists and persist in saga state ───────────────────
		AllItems = order.Items
			.Select(item => new SoldItemDto(item.ProductId, item.Quantity))
			.ToList();

		// ── Persist order item details for discount calculation in Step 1c ─
		DiscountItems = order.Items
			.Select(item => new CheckoutDiscountItemDto(
				item.ProductId,
				Guid.Empty, // CategoryId will be enriched in Step 1b
				item.Quantity,
				item.UnitPrice))
			.ToList();

		SubTotal = order.Items.Sum(i => i.TotalPrice);

		// ── Emit Step 1b: Validate stock availability ──────────────────────
		return new ValidateCheckoutStock(Id, AllItems);
	}

	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 1b — Handle Stock Validation Response
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Processes the stock validation response from the Inventory module.
	/// On success, enriches discount items with category data and emits
	/// <see cref="CalculateCheckoutDiscount"/>. On failure, terminates the saga.
	/// </summary>
	public object Handle(CheckoutStockValidated response)
	{
		if (!response.IsValid)
		{
			Status = CheckoutSagaStatus.Failed;
			FailureReason = response.ErrorMessage;
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, OrderNumber, false, FailureReason);
		}

		Status = CheckoutSagaStatus.StockValidated;

		// ── Enrich discount items with category IDs from validation ────────
		EnrichedItems = response.EnrichedItems ?? new List<EnrichedItemDto>();

		DiscountItems = DiscountItems.Select(item =>
		{
			var enriched = EnrichedItems.FirstOrDefault(e => e.ProductId == item.ProductId);
			var categoryId = enriched?.CategoryId ?? Guid.Empty;
			return item with { CategoryId = categoryId };
		}).ToList();

		// ── Emit Step 1c: Calculate discounts ──────────────────────────────
		return new CalculateCheckoutDiscount(Id, DiscountItems);
	}

	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 1c — Handle Discount Calculation Response
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Processes the discount calculation response from the Offers module.
	/// Calculates final totals, validates payment sufficiency, and emits
	/// <see cref="DeductStockForCheckout"/>. On failure, terminates the saga.
	/// </summary>
	public object Handle(CheckoutDiscountCalculated response)
	{
		if (!response.IsSuccess)
		{
			Status = CheckoutSagaStatus.Failed;
			FailureReason = response.ErrorMessage;
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, OrderNumber, false, FailureReason);
		}

		Status = CheckoutSagaStatus.DiscountCalculated;

		// ── Calculate order totals ─────────────────────────────────────────
		DiscountAmount = response.TotalDiscount;
		TotalAmount = SubTotal - DiscountAmount;

		// ── Financial Validation ───────────────────────────────────────────
		if (PaidAmount < TotalAmount && !CustomerId.HasValue)
		{
			Status = CheckoutSagaStatus.Failed;
			FailureReason = $"Insufficient payment. Required: {TotalAmount}, Provided: {PaidAmount}. Debt is not allowed for anonymous customers.";
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, OrderNumber, false, FailureReason);
		}

		// ── Emit Step 2: Deduct stock atomically ──────────────────────────
		return new DeductStockForCheckout(Id, OrderId, AllItems);
	}

	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 2 — Handle Stock Deduction Response
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Processes the stock deduction response from the Inventory module.
	/// On success, records deducted items and emits <see cref="FinalizeCheckoutOrder"/>.
	/// On failure, terminates the saga (no compensation needed since deduction failed atomically).
	/// </summary>
	public object Handle(CheckoutStockDeducted response)
	{
		if (!response.IsSuccess)
		{
			Status = CheckoutSagaStatus.Failed;
			FailureReason = response.ErrorMessage;
			MarkCompleted();
			return new CheckoutSagaCompleted(Id, OrderNumber, false, FailureReason);
		}

		Status = CheckoutSagaStatus.StockDeducted;
		DeductedItems = AllItems;

		// ── Emit Step 3: Finalize the order ────────────────────────────────
		return new FinalizeCheckoutOrder(
			Id, OrderId, PaidAmount, CustomerId,
			SubTotal, DiscountAmount, TotalAmount, AllItems);
	}

	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 3 — Handle Order Finalization Response
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Processes the order finalization response. On success, emits
	/// <see cref="PublishCheckoutCompleted"/>. On failure, emits
	/// <see cref="CompensateCheckoutStock"/> to restore deducted inventory.
	/// </summary>
	public object[] Handle(CheckoutOrderFinalized response)
	{
		if (!response.IsSuccess)
		{
			// ── COMPENSATE: Restore stock since order finalization failed ──
			Status = CheckoutSagaStatus.Compensating;
			FailureReason = response.ErrorMessage;

			MarkCompleted();
			return new object[]
			{
				new CompensateCheckoutStock(Id, OrderId, DeductedItems),
				new CheckoutSagaCompleted(Id, OrderNumber, false, FailureReason)
			};
		}

		Status = CheckoutSagaStatus.OrderCompleted;

		// ── Emit Step 4: Publish for downstream modules ───────────────────
		return new object[]
		{
			new PublishCheckoutCompleted(Id, OrderId, CustomerId, TotalAmount, PaidAmount, AllItems)
		};
	}

	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 4 — Handle Publish Completed Response
	// ═══════════════════════════════════════════════════════════════════════════

	/// <summary>
	/// Processes the event publication response. On success, marks the saga
	/// as completed. On failure, emits both <see cref="CompensateCheckoutOrder"/>
	/// and <see cref="CompensateCheckoutStock"/> to fully roll back.
	/// </summary>
	public object[] Handle(CheckoutCompletedPublished response)
	{
		if (!response.IsSuccess)
		{
			// ── COMPENSATE: Revert both order and stock ────────────────────
			Status = CheckoutSagaStatus.Compensating;
			FailureReason = response.ErrorMessage;

			MarkCompleted();
			return new object[]
			{
				new CompensateCheckoutOrder(Id, OrderId),
				new CompensateCheckoutStock(Id, OrderId, DeductedItems),
				new CheckoutSagaCompleted(Id, OrderNumber, false, FailureReason)
			};
		}

		// ── SUCCESS: Saga is complete ─────────────────────────────────────
		Status = CheckoutSagaStatus.Completed;
		CompletedAt = DateTime.UtcNow;

		MarkCompleted();
		return new object[]
		{
			new CheckoutSagaCompleted(Id, OrderNumber, true)
		};
	}
}
