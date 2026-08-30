using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Events;
using SharedContracts.Sales.Saga;
using Wolverine;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout.Saga;

/// <summary>
/// Orchestrator-based Saga for the POS Checkout flow.
///
/// Steps (happy path):
///   1. StartCheckoutSaga       → Validate stock, prepare order data
///   2. DeductStockForCheckout   → Atomically deduct inventory for ALL items
///   3. FinalizeCheckoutOrder    → Mark the order as Completed
///   4. PublishCheckoutCompleted → Raise OrderCompletedEvent for Finance
///
/// Compensation (on failure at any step):
///   - CompensateCheckoutStock → Restore all previously deducted items
///   - CompensateCheckoutOrder → Revert order status to Pending
/// </summary>
public static class CheckoutSagaOrchestrator
{
	// ═══════════════════════════════════════════════════════════════════════════
	// STEP 1: Start the Saga — validate stock and prepare order data
	// ═══════════════════════════════════════════════════════════════════════════

	[Transactional]
	public static async Task<CheckoutSagaCompleted> Handle(
		StartCheckoutSaga command,
		ISalesDataContext salesContext,
		IMessageBus bus,
		CancellationToken cancellationToken)
	{
		var saga = new CheckoutSagaState
		{
			OrderId = command.OrderId,
			PaidAmount = command.PaidAmount,
			CustomerId = command.CustomerId
		};

		// ── Load the order ─────────────────────────────────────────────────
		var order = await salesContext.Orders
			.Include(o => o.Items)
			.FirstOrDefaultAsync(o => o.OrderId == command.OrderId, cancellationToken);

		if (order is null)
		{
			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				$"Order '{command.OrderId}' not found.");
		}

		if (order.Status != OrderStatus.Pending)
		{
			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				$"Order '{command.OrderId}' is not in Pending status.");
		}

		if (!order.Items.Any())
		{
			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				"Cannot checkout an order with no items.");
		}

		// ── Prepare item lists ─────────────────────────────────────────────
		var soldItems = order.Items
			.Select(item => new SoldItemDto(item.ProductId, item.Quantity))
			.ToList();

		saga.AllItems = soldItems;

		// ── STEP 1b: Validate stock availability (pre-flight check) ───────
		var validationResponse = await bus.InvokeAsync<CheckoutStockValidated>(
			new ValidateCheckoutStock(saga.SagaId, soldItems),
			cancellationToken);

		if (!validationResponse.IsValid)
		{
			saga.Status = CheckoutSagaStatus.Failed;
			saga.FailureReason = validationResponse.ErrorMessage;

			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				saga.FailureReason);
		}

		saga.Status = CheckoutSagaStatus.StockValidated;

		// ── Calculate order totals ─────────────────────────────────────────
		saga.SubTotal = order.Items.Sum(i => i.TotalPrice);
		saga.DiscountAmount = order.DiscountAmount;
		saga.TotalAmount = saga.SubTotal - saga.DiscountAmount;

		// ── STEP 2: Deduct stock atomically ────────────────────────────────
		var deductResponse = await bus.InvokeAsync<CheckoutStockDeducted>(
			new DeductStockForCheckout(saga.SagaId, command.OrderId, soldItems),
			cancellationToken);

		if (!deductResponse.IsSuccess)
		{
			saga.Status = CheckoutSagaStatus.Failed;
			saga.FailureReason = deductResponse.ErrorMessage;

			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				saga.FailureReason);
		}

		saga.Status = CheckoutSagaStatus.StockDeducted;
		saga.DeductedItems = soldItems;

		// ── STEP 3: Finalize the order ─────────────────────────────────────
		var finalizeResponse = await bus.InvokeAsync<CheckoutOrderFinalized>(
			new FinalizeCheckoutOrder(
				saga.SagaId, command.OrderId, command.PaidAmount, command.CustomerId,
				saga.SubTotal, saga.DiscountAmount, saga.TotalAmount, soldItems),
			cancellationToken);

		if (!finalizeResponse.IsSuccess)
		{
			// ── COMPENSATE: Restore stock since order finalization failed ───
			saga.Status = CheckoutSagaStatus.Compensating;
			await bus.InvokeAsync(
				new CompensateCheckoutStock(saga.SagaId, command.OrderId, saga.DeductedItems),
				cancellationToken);

			saga.Status = CheckoutSagaStatus.Failed;
			saga.FailureReason = finalizeResponse.ErrorMessage;

			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				saga.FailureReason);
		}

		saga.Status = CheckoutSagaStatus.OrderCompleted;

		// ── STEP 4: Publish event for downstream (Finance, etc.) ───────────
		var publishResponse = await bus.InvokeAsync<CheckoutCompletedPublished>(
			new PublishCheckoutCompleted(
				saga.SagaId, command.OrderId, command.CustomerId,
				saga.TotalAmount, command.PaidAmount, soldItems),
			cancellationToken);

		if (!publishResponse.IsSuccess)
		{
			// ── COMPENSATE: Revert both order and stock ─────────────────────
			saga.Status = CheckoutSagaStatus.Compensating;

			await bus.InvokeAsync(
				new CompensateCheckoutOrder(saga.SagaId, command.OrderId),
				cancellationToken);

			await bus.InvokeAsync(
				new CompensateCheckoutStock(saga.SagaId, command.OrderId, saga.DeductedItems),
				cancellationToken);

			saga.Status = CheckoutSagaStatus.Failed;
			saga.FailureReason = publishResponse.ErrorMessage;

			return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, false,
				saga.FailureReason);
		}

		saga.Status = CheckoutSagaStatus.Completed;
		saga.CompletedAt = DateTime.UtcNow;

		return new CheckoutSagaCompleted(saga.SagaId, command.OrderId, true);
	}
}
