using Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Products.Commands.UpdatePickingStrategy
{
	public record  UpdateProductPickingStrategyCommand(Guid ProductId, PickingStrategy PickingStrategy);

}
