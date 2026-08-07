using Inventory.Application;
using Inventory.Infrastructure.DependancyInjections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
				.AddInventoryInfrastructure(configuration);
		

			return services;
		}

		public IApplicationBuilder UseModule(IApplicationBuilder app) => app;

	}
}
