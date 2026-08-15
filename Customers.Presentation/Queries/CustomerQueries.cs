using Customers.Application.Abstractions;
using Customers.Presentation.Dtos;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using SharedPresentation.GraphQL;

namespace Customers.Presentation.Queries;

[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
public class CustomerQueries
{
	/// <summary>
	/// Gets a paged, filterable, and sortable list of customers with their total debt.
	/// </summary>
	[UsePaging(IncludeTotalCount = true)]
	[UseFiltering]
	[UseSorting]
	public IQueryable<CustomerDto> GetCustomers([Service] ICustomersDataContext context)
	{
		return context.Customers
			.AsNoTracking()
			.Select(c => new CustomerDto(
				c.CustomerId,
				c.Name,
				c.Phone,
				c.TotalDebt,
				c.CreatedAt));
	}

	/// <summary>
	/// Gets a single customer by ID with their current debt state.
	/// </summary>
	[UseFirstOrDefault]
	public IQueryable<CustomerDto> GetCustomerById(
		Guid customerId,
		[Service] ICustomersDataContext context)
	{
		return context.Customers
			.AsNoTracking()
			.Where(c => c.CustomerId == customerId)
			.Select(c => new CustomerDto(
				c.CustomerId,
				c.Name,
				c.Phone,
				c.TotalDebt,
				c.CreatedAt));
	}
}
