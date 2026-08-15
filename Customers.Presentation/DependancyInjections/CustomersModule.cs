using Customers.Application;
using Customers.Infrastructure.DependancyInjections;
using Customers.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedPresentation.Common;
using System.Reflection;

namespace Customers.Presentation.DependancyInjections;

public sealed class CustomersModule : IModule
{
	public string Name => "Customers";

	public Assembly GetApplicationAssembly() => typeof(ICustomersApplicationMarker).Assembly;
	public Assembly GetPresentationAssembly() => typeof(CustomerEndpoints).Assembly;

	public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddCustomersInfrastructure(configuration)
			.AddCustomersPresentationServices()
			.AddCustomersGraphQLServices();

		return services;
	}

	public IApplicationBuilder UseModule(IApplicationBuilder app) => app;
}
