using FluentResults;
using Microsoft.AspNetCore.Http;
using Sales.Application.Features.DeleteItem;
using Sales.Application.Features.StartOrder;
using Sales.Presentation.Mapping;
using Sales.Presentation.Requests.AddItem;
using Sales.Presentation.Requests.UpdateQuantity;
using SharedPresentation.Extentions;
using Wolverine;
using Wolverine.Http;
namespace Sales.Presentation.Endpoints
{
	public static class SalesEndpoints
	{
		[WolverinePost("/api/pos/orders")]
		public static async Task<IResult> Handle(
			IMessageBus _bus)
		{
			var command= new StartOrderCommand();

			var result = await _bus.InvokeAsync<Result<string>>(command);

			return result.ToCreatedResult($"/api/orders/{result.Value}");


		}

		[WolverinePost("api/pos/orders/{orderId}/item")]
		public static async Task<IResult> Handle(Guid orderId, AddOrderItemRequest request,
			OrderMapper mapper,
			IMessageBus _bus)
		{
			var command = mapper.MapToCommand(request,orderId);
			var result = await _bus.InvokeAsync<Result<Guid>>(command);

			if (result.IsSuccess)
			{
				return Results.Created($"/api/orders/{orderId}/item/{result.Value}", result.Value);
			}
			else
			{
				return Results.BadRequest(result.Errors);
			}
		}

		[WolverinePut("api/pos/orders/{orderId}/item")]
		public static async Task<IResult> Handle(Guid orderId, UpdateQuantityItemRequest request,
			OrderMapper mapper,
			IMessageBus _bus)
		{
			var command = mapper.MapToCommand(request, orderId);
			var result = await _bus.InvokeAsync<Result>(command);

			return result.ToHttpResult();
		}

		[WolverineDelete("api/pos/orders/{orderId}/item/{orderItemId}")]
		public static async Task<IResult> Handle(Guid orderId, Guid orderItemId,
			IMessageBus _bus)
		{
			var command = new DeleteOrderItemCommand(orderId, orderItemId);
			var result = await _bus.InvokeAsync<Result>(command);
			return result.ToHttpResult();
		}
	}
}
