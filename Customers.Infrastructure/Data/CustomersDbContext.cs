using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Data;

public class CustomersDbContext : DbContext, ICustomersDataContext
{
	public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options) { }

	public DbSet<Customer> Customers { get; set; }
	public DbSet<CustomerLedger> CustomerLedgers { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasDefaultSchema("Customers");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersDbContext).Assembly);
	}
}
