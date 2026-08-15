
using FluentValidation;
using Inventory.Application.Services;
using Inventory.Presentation.Categories.Mappers;
using Inventory.Presentation.Categories.Requests;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using System.Reflection;

namespace Inventory.Presentation.DependancyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInventoryPresentationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>(ServiceLifetime.Singleton);

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(CategoryMapper))
               .AddClasses(classes => classes
                   .Where(type => type.GetCustomAttribute<MapperAttribute>() != null))
               .AsSelf()
               .WithSingletonLifetime());

            services.AddHostedService<BatchExpiryBackgroundService>();

            return services;
        }
    }
}
