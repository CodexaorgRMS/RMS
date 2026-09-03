using SharedContracts.Sales.Events;
using SharedContracts.Sales.Saga;
using Wolverine;

namespace Sales.Application.Features.Checkout.Sagaes;

/// <summary>
/// Saga Step 4: Publishes the <see cref="OrderCompletedEvent"/> for downstream
/// modules (Finance, Notifications, etc.) and returns a confirmation response.
/// </summary>
public static class PublishCheckoutCompletedHandler
{
	public static async Task<CheckoutCompletedPublished> Handle(
		PublishCheckoutCompleted command,
		IMessageBus bus)
	{
		try
		{
			await bus.PublishAsync(new OrderCompletedEvent(
				OrderId: command.OrderId,
				CustomerId: command.CustomerId,
				TotalAmount: command.TotalAmount,
				PaidAmount: command.PaidAmount,
				Items: command.Items,
				CompletedAt: DateTime.UtcNow));

			return new CheckoutCompletedPublished(command.SagaId, IsSuccess: true);
		}
		catch (Exception ex)
		{
			return new CheckoutCompletedPublished(
				command.SagaId,
				IsSuccess: false,
				ErrorMessage: $"Failed to publish checkout completed event: {ex.Message}");
		}
	}
}
