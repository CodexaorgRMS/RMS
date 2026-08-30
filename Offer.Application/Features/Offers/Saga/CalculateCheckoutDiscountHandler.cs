using Microsoft.EntityFrameworkCore;
using Offers.Application.Abstractions;
using Offers.Domain.Models;
using SharedContracts.Sales.Saga;

namespace Offers.Application.Features.Offers.Saga;

/// <summary>
/// Saga Step 1c: Calculates applicable discounts for the checkout order
/// using the internal OfferEngine.
/// </summary>
public static class CalculateCheckoutDiscountHandler
{
	public static async Task<CheckoutDiscountCalculated> Handle(
		CalculateCheckoutDiscount command,
		IOffersDataContext context,
		IOfferEngine offerEngine,
		CancellationToken cancellationToken)
	{
		try
		{
			var now = DateTime.UtcNow;

			// Fetch all active offers from the database
			var activeOffers = await context.Offers
				.Include(o => o.Targets)
				.Where(o => o.IsActive && o.StartDate <= now && o.EndDate >= now)
				.ToListAsync(cancellationToken);

			// Convert saga DTO to internal CartItemInput
			var cartItems = command.Items.Select(item => new CartItemInput(
				ProductId: item.ProductId,
				CategoryId: item.CategoryId,
				Quantity: item.Quantity,
				UnitPrice: item.UnitPrice
			)).ToList();

			// Evaluate discounts
			var evaluation = offerEngine.EvaluateOffers(activeOffers, cartItems);

			return new CheckoutDiscountCalculated(
				command.SagaId, 
				IsSuccess: true, 
				TotalDiscount: evaluation.TotalDiscount);
		}
		catch (Exception ex)
		{
			return new CheckoutDiscountCalculated(
				command.SagaId,
				IsSuccess: false,
				TotalDiscount: 0m,
				ErrorMessage: $"Failed to calculate discount: {ex.Message}");
		}
	}
}
