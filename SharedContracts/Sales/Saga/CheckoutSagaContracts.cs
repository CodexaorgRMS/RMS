using SharedContracts.Sales.Events;

namespace SharedContracts.Sales.Saga;

// ═══════════════════════════════════════════════════════════════════════════════
// Saga Status Enum
// ═══════════════════════════════════════════════════════════════════════════════

public enum CheckoutSagaStatus
{
	NotStarted,
	StockValidated,
	DiscountCalculated,
	StockDeducted,
	OrderCompleted,
	FinanceRecorded,
	Completed,
	Compensating,
	Failed
}

// ═══════════════════════════════════════════════════════════════════════════════
// Step 1 — Initiating Command (triggers the Saga via Starts<>)
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Initiates the Checkout Saga. Sent from the endpoint.
/// The saga's <c>Starts</c> method will load the order, validate it,
/// and emit <see cref="ValidateCheckoutStock"/> as its first cascading message.
/// </summary>
public record StartCheckoutSaga(Guid SagaId, string OrderNumber, decimal PaidAmount, Guid? CustomerId);

// ═══════════════════════════════════════════════════════════════════════════════
// Step 1b — Stock Validation (Request → Response)
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>Step 1b: Validate stock availability for all items via the Inventory module.</summary>
public record ValidateCheckoutStock(Guid SagaId, List<SoldItemDto> Items);

/// <summary>Response from Inventory module confirming stock validity.</summary>
public record EnrichedItemDto(Guid ProductId, Guid CategoryId);
public record CheckoutStockValidated(Guid SagaId, bool IsValid, string? ErrorMessage = null, List<EnrichedItemDto>? EnrichedItems = null);

// ═══════════════════════════════════════════════════════════════════════════════
// Step 1c — Discount Calculation (Request → Response)
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>Step 1c: Calculate applicable discounts via the Offers module.</summary>
public record CalculateCheckoutDiscount(Guid SagaId, List<CheckoutDiscountItemDto> Items);
public record CheckoutDiscountItemDto(Guid ProductId, Guid CategoryId, int Quantity, decimal UnitPrice);

/// <summary>Response from Offers module with discount totals.</summary>
public record CheckoutDiscountCalculated(Guid SagaId, bool IsSuccess, decimal TotalDiscount, string? ErrorMessage = null);

// ═══════════════════════════════════════════════════════════════════════════════
// Step 2 — Stock Deduction (Request → Response)
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>Step 2: Deduct stock from inventory for all items atomically.</summary>
public record DeductStockForCheckout(Guid SagaId, Guid OrderId, List<SoldItemDto> Items);

/// <summary>Response from Inventory confirming deduction success.</summary>
public record CheckoutStockDeducted(Guid SagaId, bool IsSuccess, string? ErrorMessage = null);

// ═══════════════════════════════════════════════════════════════════════════════
// Step 3 — Order Finalization (Request → Response)
// ═══════════════════════════════════════════════════════════════════════════════

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

/// <summary>Response confirming order finalization.</summary>
public record CheckoutOrderFinalized(Guid SagaId, bool IsSuccess, string? ErrorMessage = null);

// ═══════════════════════════════════════════════════════════════════════════════
// Step 4 — Publish Downstream Event (Request → Response)
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>Step 4: Publish the completed event for Finance and other downstream modules.</summary>
public record PublishCheckoutCompleted(
	Guid SagaId,
	Guid OrderId,
	Guid? CustomerId,
	decimal TotalAmount,
	decimal PaidAmount,
	List<SoldItemDto> Items);

/// <summary>Response confirming event publication.</summary>
public record CheckoutCompletedPublished(Guid SagaId, bool IsSuccess, string? ErrorMessage = null);

// ═══════════════════════════════════════════════════════════════════════════════
// Compensation Commands
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>Compensate: restore all previously deducted stock.</summary>
public record CompensateCheckoutStock(Guid SagaId, Guid OrderId, List<SoldItemDto> DeductedItems);

/// <summary>Compensate: revert the order status back to Pending.</summary>
public record CompensateCheckoutOrder(Guid SagaId, Guid OrderId);

// ═══════════════════════════════════════════════════════════════════════════════
// Saga Terminal Response
// ═══════════════════════════════════════════════════════════════════════════════

/// <summary>
/// Final outcome of the Checkout Saga, returned to the caller (endpoint).
/// </summary>
public record CheckoutSagaCompleted(Guid SagaId, string OrderNumber, bool IsSuccess, string? ErrorMessage = null);
