using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Presentation.Requests.UpdateQuantity
{
	public record UpdateQuantityItemRequest(Guid orderItemId, int Quantity);
	
}
