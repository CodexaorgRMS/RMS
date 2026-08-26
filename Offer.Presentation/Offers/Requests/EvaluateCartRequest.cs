namespace Offers.Presentation.Offers.Requests;

public sealed record CartItemRequest(
    Guid ProductId,
    Guid CategoryId,
    int Quantity,
    decimal UnitPrice);

public sealed record EvaluateCartRequest(List<CartItemRequest> Items);
