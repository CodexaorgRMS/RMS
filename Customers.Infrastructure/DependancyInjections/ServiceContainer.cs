using Customers.Application.Abstractions;
using Customers.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.EntityFrameworkCore;

namespace Customers.Infrastructure.DependancyInjections;

public static class ServiceContainer
{
	public static IServiceCollection AddCustomersInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Constr");
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new InvalidOperationException("Connection string 'Constr' not found in configuration.");
		}

		services.AddDbContextWithWolverineIntegration<CustomersDbContext>((sp, options) =>
		{
			options.UseSqlServer(connectionString, sqlOptions =>
			{
				sqlOptions.MigrationsAssembly(typeof(CustomersDbContext).Assembly.GetName().Name);
			});
		});

		services.AddScoped<ICustomersDataContext>(sp => sp.GetRequiredService<CustomersDbContext>());

		return services;
	}
}
