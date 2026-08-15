using Customers.Presentation.Mapping;
using Customers.Presentation.Requests;
using FluentResults;
using Microsoft.AspNetCore.Http;
using SharedPresentation.Extentions;
using Wolverine;
using Wolverine.Http;

namespace Customers.Presentation.Endpoints
{
	public static class CustomerEndpoints
	{
		[WolverinePost("/api/customers")]
		public static async Task<IResult> CreateCustomer(
			CreateCustomerRequest request,
			CustomerMapper mapper,
			IMessageBus bus)
		{
			var command = mapper.MapToCommand(request);
			var result = await bus.InvokeAsync<Result<Guid>>(command);

			return result.ToCreatedResult($"/api/customers/{result.Value}");
		}


		[WolverinePost("/api/customers/{customerId}/payments")]
		public static async Task<IResult> AddPayment(
			Guid customerId,
			AddCustomerPaymentRequest request,
			CustomerMapper mapper,
			IMessageBus bus)
		{
			var command = mapper.MapToCommand(request, customerId);
			var result = await bus.InvokeAsync<Result<Guid>>(command);

			return result.ToCreatedResult($"/api/customers/{customerId}/payments/{result.Value}");
		}

		[WolverinePost("/api/customers/{customerId}/debts")]
		public static async Task<IResult> AddManualDebt(
			Guid customerId,
			AddManualCustomerDebtRequest request,
			CustomerMapper mapper,
			IMessageBus bus)
		{
			var command = mapper.MapToCommand(request, customerId);
			var result = await bus.InvokeAsync<Result<Guid>>(command);

			return result.ToCreatedResult($"/api/customers/{customerId}/debts/{result.Value}");
		}
	}
}
