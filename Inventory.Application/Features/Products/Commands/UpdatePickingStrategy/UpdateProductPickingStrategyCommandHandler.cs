using FluentResults;
using Inventory.Application.Abstractions;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.UpdatePickingStrategy
{
	[Transactional]
	public static class UpdateProductPickingStrategyCommandHandler
	{
		public static async Task<Result> Handle(UpdateProductPickingStrategyCommand command, 
			IInventoryDataContext context)
		{ 
			var product = await context.Products.FindAsync(command.ProductId);
			if (product == null)
			{
				return Result.Fail($"Product not found.");
			}
			product.CustomPickingStrategy = command.PickingStrategy;
			return Result.Ok();
		}
	}
}
