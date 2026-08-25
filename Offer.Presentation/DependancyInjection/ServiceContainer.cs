using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Offers.Presentation.Offers.Mappers;
using Offers.Presentation.Offers.Requests.Validators;
using Riok.Mapperly.Abstractions;

namespace Offers.Presentation.DependancyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddOffersPresentationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreatePercentageOfferRequestValidator>(ServiceLifetime.Singleton);

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(OfferMapper))
            .AddClasses(classes => classes
                .Where(type => type.GetCustomAttribute<MapperAttribute>() != null))
            .AsSelf()
            .WithSingletonLifetime());

        return services;
    }
}