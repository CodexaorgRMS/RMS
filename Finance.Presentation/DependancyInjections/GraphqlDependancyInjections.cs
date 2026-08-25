using System.Reflection;
using Finance.Infrastructure.Data;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Presentation.DependancyInjections;

public static class GraphqlDependancyInjections
{
    public static IServiceCollection AddFinanceGraphQLServices(this IServiceCollection services)
    {
        var builder = services.AddGraphQLServer()
            .RegisterDbContextFactory<FinanceDbContext>()
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
