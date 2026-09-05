using Sales.Application.Abstractions;
using Sales.Domain.Entities;
using SharedContracts.Sales.Saga;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout.Sagaes;

/// <summary>
/// Handles the <see cref="CheckoutSagaCompleted"/> cascading message emitted
/// by the <see cref="CheckoutSagaOrchestrator"/> at every terminal state.
///
/// Persists the saga outcome to the <see cref="CheckoutResult"/> table so the
/// GET status endpoint can query the result after the saga has been deleted
/// by Wolverine's MarkCompleted().
/// </summary>
[Transactional]
public static class CheckoutSagaCompletedHandler
{
	public static async Task Handle(
		CheckoutSagaCompleted message,
		ISalesDataContext context,
		CancellationToken cancellationToken)
	{
		var result = new CheckoutResult
		{
			CheckoutResultId = Guid.NewGuid(),
			SagaId = message.SagaId,
			OrderNumber = message.OrderNumber,
			IsSuccess = message.IsSuccess,
			ErrorMessage = message.ErrorMessage,
			CompletedAt = DateTime.UtcNow
		};

		await context.CheckoutResults.AddAsync(result, cancellationToken);
	}
}
