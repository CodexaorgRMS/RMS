using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Inventory.Infrastructure.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInventoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			// Register your infrastructure services here
			// Example: services.AddDbContext<YourDbContext>(options => ...);
			return services;
		}
	}
}
