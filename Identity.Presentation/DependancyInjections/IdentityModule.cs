using Identity.Application;
using Identity.Infrastucture.DependancyInjections;
using Identity.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedPresentation.Common;
using System.Reflection;
namespace Identity.Presentation.DependancyInjections;

public sealed class IdentityModule : IModule
{
	public string Name => "Customers";

	public Assembly GetApplicationAssembly() => typeof(IIdentityApplicationMarker).Assembly;
	public Assembly GetPresentationAssembly() => typeof(AuthEndpoints).Assembly;

	public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddIdntityInfrastructure(configuration)
			.AddIdentityPresentationServices()
			.AddIdentityGraphQLServices();

		return services;
	}

	public IApplicationBuilder UseModule(IApplicationBuilder app) => app;
}
