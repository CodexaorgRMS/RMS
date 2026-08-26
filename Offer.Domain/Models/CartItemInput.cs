namespace Offers.Domain.Models;

public sealed record CartItemInput(
    Guid ProductId,
    Guid CategoryId,
    int Quantity,
    decimal UnitPrice);
