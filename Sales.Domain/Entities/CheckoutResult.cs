namespace Sales.Domain.Entities;

/// <summary>
/// Persists the final outcome of a Checkout Saga.
/// Since Wolverine deletes saga state on MarkCompleted(), this entity
/// serves as the durable record that the GET status endpoint queries.
/// </summary>
public class CheckoutResult
{
	public Guid CheckoutResultId { get; set; }
	public Guid SagaId { get; set; }
	public string OrderNumber { get; set; } = string.Empty;
	public bool IsSuccess { get; set; }
	public string? ErrorMessage { get; set; }
	public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
