using FluentResults;
using Inventory.Application.Abstractions;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.UpdatePickingStrategy
{
	[Transactional]
	public static class UpdateCategoryPickingStrategyCommandHandler
	{
		public static async Task<Result> Handle(UpdateCategoryPickingStrategyCommand command, 
			IInventoryDataContext context)
		{ 
			var category = await context.Categories.FindAsync(command.CategoryId);

			if (category == null)
			{
				return Result.Fail($"Category  not found.");
			}

			category.DefaultPickingStrategy = command.PickingStrategy;

			return Result.Ok();
		}
	}
}
