using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Sales.Infrastructure.Data;
using System.Reflection;

namespace Sales.Presentation.DependancyInjections
{
	public static class GraphqlDependancyInjections
	{
		public static IServiceCollection AddSalesGraphQLServices(this IServiceCollection services)
		{
			var builder = services.AddGraphQLServer()
				.RegisterDbContextFactory<SalesDbContext>()
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
}
