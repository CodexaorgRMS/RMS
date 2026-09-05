using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Inventory.Interfaces;

namespace Sales.Application.Features.AddOrderItem
{
	public class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
	{
		private readonly ISalesDataContext _context;
		private readonly IProductService _productService;
		private readonly IInventoryService _inventoryService;
		public AddOrderItemCommandValidator(ISalesDataContext context,
			IProductService productService,
			IInventoryService inventoryService)
		{
			_context = context;
			_productService = productService;
			_inventoryService = inventoryService;

			RuleFor(x => x.orderNumber)
				.NotEmpty()
				.WithMessage("OrderNumber is required.")
				.MustAsync(OrderExists)
				.WithMessage("Order not found.")
				.MustAsync(IsOrderPended)
				.WithMessage("Order is not in a pending state.");


			RuleFor(x => x.ProductId)
				.NotEmpty()
				.WithMessage("ProductId is required.")
				.MustAsync(ProductExists)
				.WithMessage("Product not found.");

			RuleFor(x => x.Quantity)
				.GreaterThan(0)
				.WithMessage("Quantity must be greater than zero.")
				.CustomAsync(async (quantity, context, cancellationToken) =>
				{
					var command = context.InstanceToValidate;

					var availableStock =
						await _inventoryService.AvailableStock(command.ProductId);

					if (!availableStock.HasValue)
					{
						context.AddFailure(
							"Quantity",
							"Unable to determine available stock.");

						return;
					}

					if (availableStock.Value < quantity)
					{
						context.AddFailure(
							"Quantity",
							$"Insufficient stock. Available quantity: {availableStock.Value}.");
					}
				});
		}


		private async Task<bool> OrderExists(string orderNumber, CancellationToken cancellationToken)
		{
			return await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber, cancellationToken);
		}

	    private async Task<bool> IsOrderPended(string orderNumber, CancellationToken cancellationToken)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
			return order != null && order.Status == OrderStatus.Pending;
		}

		private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
		{
			return await _productService.ProductExistsAsync(productId);
		}
	}
}
