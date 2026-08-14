using Inventory.Domain.Enums;

namespace Inventory.Application.Features.Categories.Commands.UpdatePickingStrategy
{
	public record  UpdateCategoryPickingStrategyCommand(Guid CategoryId,PickingStrategy PickingStrategy);
	
}
