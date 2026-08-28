namespace Customers.Presentation.Requests;

public record AddManualCustomerDebtRequest(decimal Amount, Guid? RefrenceOrderId, string? Reason);
