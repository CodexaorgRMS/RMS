using Inventory.Application.Abstractions;
using Inventory.Domain.Enums;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Services
{
	public class PickingStrategyFactory : IPickingStrategyFactory
	{
		public IInventoryPickingStrategy GetStrategy(PickingStrategy strategyEnum)
		{
			return strategyEnum switch
			{
				PickingStrategy.FEFO => new FefoPickingStrategy(),
				PickingStrategy.FIFO => new FifoPickingStrategy(),
				PickingStrategy.LIFO => new LifoPickingStrategy(),
				PickingStrategy.HIFO => new HifoPickingStrategy(),

				_ => new FefoPickingStrategy()
			};
		}
	}
}
