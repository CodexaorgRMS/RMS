using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application;
using Sales.Infrastructure.DependancyInjections;
using Sales.Presentation.Endpoints;
using SharedPresentation.Common;
using System.Reflection;

namespace Sales.Presentation.DependancyInjections
{
	public sealed class SalesModule : IModule
	{
		public string Name => "Sales";

		public Assembly GetApplicationAssembly() => typeof(ISalesMarker).Assembly;
		public Assembly GetPresentationAssembly() => typeof(SalesEndpoints).Assembly;
		public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddSalesInfrastructure(configuration)
				.AddSalesPresentationServices()
				.AddSalesGraphQLServices();

			return services;
		}

		public IApplicationBuilder UseModule(IApplicationBuilder app) => app;
	}
}
