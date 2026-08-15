using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedPresentation.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Sales.Infrastructure.DependancyInjections;
namespace Sales.Presentation.DependancyInjections
{
	public sealed class SalesModule : IModule
	{
		public string Name => "Sales";

		public Assembly GetApplicationAssembly() => typeof(SalesModule).Assembly;
		public Assembly GetPresentationAssembly() => typeof(SalesModule).Assembly;
		public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddSalesInfrastructure(configuration);

			return services;
		}

		public IApplicationBuilder UseModule(IApplicationBuilder app) => app;

	}
}
