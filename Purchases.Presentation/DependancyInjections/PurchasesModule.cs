using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchases.Application;
using Purchases.Infrastructure.DependancyInjections;
using Sales.Presentation;
using SharedPresentation.Common;
using System.Reflection;

namespace Purchases.Presentation.DependancyInjections
{
    public class PurchasesModule : IModule
    {
        public string Name => "Purchases";

        public Assembly GetApplicationAssembly() => typeof(IPurchasesApplicationMarker).Assembly;

        public Assembly GetPresentationAssembly() => typeof(IPurchasesPresentationMarker).Assembly;

        public IServiceCollection RegisterModule(IServiceCollection services,IConfiguration configuration)
        {
            services
                .AddPurchasesInfrastructure(configuration)
                .AddPurchasesGraphQLServices();
            return services;
        }

        public IApplicationBuilder UseModule(IApplicationBuilder app)
        {
            return app;
        }
    }
}
