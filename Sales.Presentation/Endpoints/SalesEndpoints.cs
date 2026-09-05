using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Application.Features.DeleteItem;
using Sales.Application.Features.Refund;
using Sales.Application.Features.StartOrder;
using Sales.Presentation.Mapping;
using Sales.Presentation.Requests.AddItem;
using Sales.Presentation.Requests.Checkout;
using Sales.Presentation.Requests.UpdateQuantity;
using SharedPresentation.Extentions;
using SharedContracts.Sales.Saga;
using Wolverine;
using Wolverine.Http;
namespace Sales.Presentation.Endpoints
{
	public static class SalesEndpoints
	{
		[WolverinePost("/api/pos/orders/{customerId}")]
		public static async Task<IResult> Handle(
			Guid customerId,
			IMessageBus _bus)
		{
			var command= new StartOrderCommand(customerId);

			var result = await _bus.InvokeAsync<Result<string>>(command);

			return result.ToCreatedResult($"/api/orders/{result.Value}");


		}

		[WolverinePost("api/pos/orders/{orderNumber}/item")]
		public static async Task<IResult> Handle(string orderNumber, AddOrderItemRequest request,
			OrderMapper mapper,
			IMessageBus _bus)
		{
			var command = mapper.MapToCommand(request,orderNumber);
			var result = await _bus.InvokeAsync<Result<Guid>>(command);

			if (result.IsSuccess)
			{
				return Results.Created($"/api/orders/{orderNumber}/item/{result.Value}", result.Value);
			}
			else
			{
				return Results.BadRequest(result.Errors);
			}
		}

		[WolverinePut("api/pos/orders/{orderNumber}/item")]
		public static async Task<IResult> Handle(string orderNumber, UpdateQuantityItemRequest request,
			OrderMapper mapper,
			IMessageBus _bus)
		{
			var command = mapper.MapToCommand(request, orderNumber);
			var result = await _bus.InvokeAsync<Result>(command);

			return result.ToHttpResult();
		}

		[WolverineDelete("api/pos/orders/{orderNumber}/item/{orderItemId}")]
		public static async Task<IResult> Handle(string orderNumber, Guid orderItemId,
			IMessageBus _bus)
		{
			var command = new DeleteOrderItemCommand(orderNumber, orderItemId);
			var result = await _bus.InvokeAsync<Result>(command);
			return result.ToHttpResult();
		}

		// ═══════════════════════════════════════════════════════════════════════
		// CHECKOUT — Async fire-and-forget via Wolverine Stateful Saga
		// ═══════════════════════════════════════════════════════════════════════

		/// <summary>
		/// Initiates the checkout saga asynchronously. Returns 202 Accepted
		/// with the SagaId for status polling via GET.
		/// </summary>
		[WolverinePost("api/pos/orders/{orderNumber}/checkout")]
		public static async Task<IResult> Handle(string orderNumber, CheckoutOrderRequest request,
			IMessageBus _bus)
		{
			var sagaId = Guid.NewGuid();
			var sagaCommand = new StartCheckoutSaga(sagaId, orderNumber, request.PaidAmount, request.CustomerId);

			// Fire-and-forget: the saga processes asynchronously via cascading messages
			await _bus.SendAsync(sagaCommand);

			// Return 202 Accepted with the SagaId for status polling
			return Results.Accepted(
				$"/api/pos/checkout/{sagaId}/status",
				new { SagaId = sagaId, OrderNumber = orderNumber, Status = "Processing" });
		}

		/// <summary>
		/// Polls the checkout saga result. Returns the outcome once the saga
		/// has completed, or 404 if the saga is still processing.
		/// </summary>
		[WolverineGet("api/pos/checkout/{sagaId}/status")]
		public static async Task<IResult> GetCheckoutStatus(
			Guid sagaId,
			ISalesDataContext context,
			CancellationToken cancellationToken)
		{
			var result = await context.CheckoutResults
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.SagaId == sagaId, cancellationToken);

			if (result is null)
			{
				// Saga is still processing — no result persisted yet
				return Results.Ok(new
				{
					SagaId = sagaId,
					Status = "Processing",
					Message = "The checkout is still being processed. Please poll again."
				});
			}

			if (!result.IsSuccess)
			{
				return Results.BadRequest(new
				{
					result.SagaId,
					result.OrderNumber,
					Status = "Failed",
					result.ErrorMessage,
					result.CompletedAt
				});
			}

			return Results.Ok(new
			{
				result.SagaId,
				result.OrderNumber,
				Status = "Completed",
				result.CompletedAt
			});
		}

		[WolverinePost("api/pos/orders/{orderId}/refund")]
		public static async Task<IResult> HandleRefund(Guid orderId,
			IMessageBus _bus)
		{
			var command = new RefundOrderCommand(orderId);
			var result = await _bus.InvokeAsync<Result>(command);
			return result.ToHttpResult();
		}

	}
}
