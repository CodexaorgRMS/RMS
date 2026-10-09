using HotChocolate.Types;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Presentation.DependancyInjections;

public static class GraphqlDependancyInjections
{
	public static IServiceCollection AddIdentityGraphQLServices(this IServiceCollection services)
	{
		var builder = services.AddGraphQLServer()
			.RegisterDbContextFactory<IdentityDbContext>()
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
