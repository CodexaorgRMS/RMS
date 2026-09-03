namespace Customers.Presentation.Requests;

public record AddManualCustomerDebtRequest(decimal Amount, Guid? SourceOrderId, string? Reason);
