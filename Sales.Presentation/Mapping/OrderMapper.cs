using Riok.Mapperly.Abstractions;
using Sales.Application.Features.AddOrderItem;
using Sales.Application.Features.Checkout;
using Sales.Application.Features.UpdateQuentity;
using Sales.Presentation.Requests.AddItem;
using Sales.Presentation.Requests.Checkout;
using Sales.Presentation.Requests.UpdateQuantity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Presentation.Mapping
{
	[Mapper]
	public partial class OrderMapper
	{
		public partial AddOrderItemCommand MapToCommand(AddOrderItemRequest request, Guid orderId);

		public partial UpdateQuantityItemCommand MapToCommand(UpdateQuantityItemRequest request, Guid orderId);

		public partial CheckoutOrderCommand MapToCommand(CheckoutOrderRequest request, Guid orderId);

	}
}
