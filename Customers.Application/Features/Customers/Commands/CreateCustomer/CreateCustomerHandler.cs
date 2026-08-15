using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using FluentResults;

namespace Customers.Application.Features.Customers.Commands.CreateCustomer;

public static class CreateCustomerHandler
{
	public static async Task<Result<Guid>> Handle(
		CreateCustomerCommand command,
		ICustomersDataContext context,
		CancellationToken cancellationToken)
	{
		var customer = new Customer
		{
			Name = command.Name,
			Phone = command.Phone,
			TotalDebt = 0,
			CreatedAt = DateTime.UtcNow
		};

		context.Customers.Add(customer);
		await context.SaveChangesAsync(cancellationToken);

		return Result.Ok(customer.CustomerId);
	}
}
