using Offers.Domain.Models;

namespace Offers.Application.Features.Offers.Commands.EvaluateCart;

public sealed record EvaluateCartOffersCommand(List<CartItemInput> Items);
