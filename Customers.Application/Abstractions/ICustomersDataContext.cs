using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.Application.Abstractions;

public interface ICustomersDataContext
{
	DbSet<Customer> Customers { get; }
	DbSet<CustomerLedger> CustomerLedgers { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
