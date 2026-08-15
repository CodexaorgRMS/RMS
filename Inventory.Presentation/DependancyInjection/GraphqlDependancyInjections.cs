using HotChocolate.Types;
using Inventory.Infrastructure.Data;
using Inventory.Presentation.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Inventory.Presentation.DependancyInjection
{
	public static class GraphqlDependancyInjections
	{
		public static IServiceCollection AddInventoryGraphQLServices(this IServiceCollection services)
		{
			var builder = services.AddGraphQLServer()
				.AddAuthorization()
				.RegisterDbContextFactory<InventoryDbContext>()
				.AddQueryType<Query>()                                 // Root Query
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
