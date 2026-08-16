namespace Sales.Application.Features.Checkout;

public record CheckoutOrderCommand(Guid orderId, decimal PaidAmount, Guid? CustomerId);
