using System.Reflection;
using Finance.Application;
using Finance.Infrastructure.DependancyInjections;
using Finance.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedPresentation.Common;

namespace Finance.Presentation.DependancyInjections;

public sealed class FinanceModule : IModule
{
    public string Name => "Finance";

    public Assembly GetApplicationAssembly() => typeof(IFinanceApplicationMarker).Assembly;
    public Assembly GetPresentationAssembly() => typeof(FinanceEndpoints).Assembly;

    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFinanceInfrastructure(configuration)
            .AddFinancePresentationServices()
            .AddFinanceGraphQLServices();

        return services;
    }

    public IApplicationBuilder UseModule(IApplicationBuilder app) => app;
}
