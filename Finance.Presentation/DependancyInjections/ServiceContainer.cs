using System.Reflection;
using Finance.Presentation.Mapping;
using Finance.Presentation.Requests.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;

namespace Finance.Presentation.DependancyInjections;

public static class ServiceContainer
{
    public static IServiceCollection AddFinancePresentationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<OpenShiftRequestValidator>(ServiceLifetime.Singleton);

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(FinanceMapper))
            .AddClasses(classes => classes
                .Where(type => type.GetCustomAttribute<MapperAttribute>() != null))
            .AsSelf()
            .WithSingletonLifetime());

        return services;
    }
}
