namespace Customers.Application.Features.Payments.Commands.AddCustomerPayment;

public record AddCustomerPaymentCommand(Guid CustomerId, decimal PaidAmount);
