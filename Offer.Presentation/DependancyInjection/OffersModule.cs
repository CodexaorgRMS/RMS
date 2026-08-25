using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offers.Application;
using Offers.Infrastructure.DependancyInjections;
using SharedPresentation.Common;
using System.Reflection;

namespace Offers.Presentation.DependancyInjection;

public sealed class OffersModule : IModule
{
    public string Name => "Offers";

    public Assembly GetApplicationAssembly() => typeof(IOffersApplicationMarker).Assembly;
    public Assembly GetPresentationAssembly() => typeof(OffersModule).Assembly;

    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOffersInfrastructure(configuration)    
            .AddOffersPresentationServices()       
            .AddOffersGraphQLServices();         

        return services;
    }

    public IApplicationBuilder UseModule(IApplicationBuilder app) => app;
}