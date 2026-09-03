using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Purchases.Application;
using Purchases.Application.Features.Purchases.Commands.Receive;
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

            // Register all validators in the Purchases.Application assembly with default Scoped lifetime
            services.AddValidatorsFromAssemblyContaining<ReceivePurchaseCommandValidator>();

            // Register ReceivePurchaseCommandValidator as Singleton to allow root provider resolution by Wolverine
            services.Replace(ServiceDescriptor.Singleton<IValidator<ReceivePurchaseCommand>, ReceivePurchaseCommandValidator>());

            return services;
        }

        public IApplicationBuilder UseModule(IApplicationBuilder app)
        {
            return app;
        }
    }
}
