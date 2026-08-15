using Inventory.Application;
using Inventory.Infrastructure.DependancyInjections;
using Inventory.Presentation.InventoryItems.Mappers;
using Inventory.Presentation.Products.Mappers;
using Inventory.Presentation.StockMovements.Mappers;
using JasperFx.Core.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using SharedPresentation.Common;
using System.Reflection;

namespace Inventory.Presentation.DependancyInjection
{

	public sealed class InventoryModule : IModule
	{
		public string Name => "Inventory";

		public Assembly GetApplicationAssembly()=> typeof(IInventoryApplicationMarker).Assembly;
		public Assembly GetPresentationAssembly() => typeof(InventoryModule).Assembly;
		public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddInventoryInfrastructure(configuration)
				.AddInventoryPresentationServices()
				.AddInventoryGraphQLServices();

			return services;
		}

		public IApplicationBuilder UseModule(IApplicationBuilder app) => app;

	}
}
