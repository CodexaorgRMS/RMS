using Customers.Infrastructure.Data;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Customers.Presentation.DependancyInjections;

public static class GraphqlDependancyInjections
{
	public static IServiceCollection AddCustomersGraphQLServices(this IServiceCollection services)
	{
		var builder = services.AddGraphQLServer()
			.RegisterDbContextFactory<CustomersDbContext>()
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
