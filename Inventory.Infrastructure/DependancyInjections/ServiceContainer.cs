using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Inventory.Application.Abstractions;
using Inventory.Infrastructure.Data;
using Wolverine.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Inventory.Infrastructure.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInventoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			// Register your infrastructure services here
			// Example: services.AddDbContext<YourDbContext>(options => ...);

			var connectionString = configuration.GetConnectionString("Constr");
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("Connection string 'Constr' not found in appsettings.json. Check your configuration.");
			}

			services.AddDbContextWithWolverineIntegration<InventoryDbContext>((sp,
			options) =>
			{

				options.UseSqlServer(connectionString, sqlOptions =>
				{
					sqlOptions.MigrationsAssembly(typeof(InventoryDbContext).Assembly.GetName().Name);
				});
			});



			services.AddScoped<IInventoryDbContext,IInventoryDbContext>();

			return services;
		}
	}
}

// Make sure your .csproj file includes the following package reference:
/*
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.0" />
</ItemGroup>
*/
