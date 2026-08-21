using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Purchases.Infrastructure.Data;
using System.Linq;
using System.Reflection;

namespace Purchases.Presentation.DependancyInjections;

public static class GraphqlDependancyInjections
{
    public static IServiceCollection AddPurchasesGraphQLServices(this IServiceCollection services)
    {
        var builder = services.AddGraphQLServer()
            .RegisterDbContextFactory<PurchasesDbContext>()
            .AddProjections()
            .AddPagingArguments()
            .AddSorting()
            .AddFiltering();

        var assembly = Assembly.GetExecutingAssembly();
        var extensionTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract &&
                        t.GetCustomAttribute<ExtendObjectTypeAttribute>() != null);

        foreach (var type in extensionTypes)
        {
            builder.AddTypeExtension(type);
        }

        return services;
    }
}
