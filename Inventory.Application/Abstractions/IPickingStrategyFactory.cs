using Inventory.Domain.Enums;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.Abstractions
{
	public interface IPickingStrategyFactory
	{
		IInventoryPickingStrategy GetStrategy(PickingStrategy strategyEnum);
	}
}
