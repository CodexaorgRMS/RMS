namespace Customers.Presentation.Requests;

public record AddManualCustomerDebtRequest(decimal Amount, string? Reason);
