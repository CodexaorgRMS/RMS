using Inventory.Application.Abstractions;
using Inventory.Application.Features.Products.SharedServices;
using Inventory.Application.Features.Stocks.SharedServices;
using Inventory.Application.Services;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedContracts.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.EntityFrameworkCore;

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


            // Register IInventoryDbContext to resolve to InventoryDbContext to avoid conflict with Wolverine's DbContext registration
            services.AddScoped<IInventoryDataContext>(provider => provider.GetRequiredService<InventoryDbContext>());


			services.AddSingleton<IPickingStrategyFactory, PickingStrategyFactory>();

			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IInventoryService, InventoryService>();

			return services;
		}
	}
}


