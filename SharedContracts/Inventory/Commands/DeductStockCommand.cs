using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Commands
{
	public record DeductStockCommand(Guid ProductId, int Quantity, Guid OrderId);

}
