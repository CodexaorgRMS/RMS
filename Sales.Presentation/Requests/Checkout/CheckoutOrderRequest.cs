namespace Sales.Presentation.Requests.Checkout;

public record CheckoutOrderRequest(decimal PaidAmount, Guid? CustomerId);
