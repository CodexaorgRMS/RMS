using System.Reflection;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Offers.Infrastructure.Data;

namespace Offers.Presentation.DependancyInjection;

public static class GraphqlDependancyInjections
{
    public static IServiceCollection AddOffersGraphQLServices(this IServiceCollection services)
    {
        var builder = services.AddGraphQLServer()
            .RegisterDbContextFactory<OffersDbContext>()
            .AddProjections()
            .AddPagingArguments()
            .AddSorting()
            .AddFiltering();

        var assembly = Assembly.GetExecutingAssembly();
        var extensionTypes = assembly.GetTypes()
            .Where(t => t.IsClass &&
                       !t.IsAbstract &&
                       t.GetCustomAttribute<ExtendObjectTypeAttribute>() != null);

        foreach (var type in extensionTypes)
        {
            builder.AddTypeExtension(type);
        }

        return services;
    }
}
