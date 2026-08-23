using Microsoft.Extensions.DependencyInjection;

namespace SharedPresentation.GraphQL
{
	public static class GraphQLSharedExtensions
	{
		public static IServiceCollection AddSharedGraphQLServices(this IServiceCollection services)
		{
			services.AddGraphQLServer()
				.AddAuthorization()
				.AddQueryType<Query>()
				.AddSubscriptionType<Subscription>()
				.AddInMemorySubscriptions()
				.AddProjections()
				.AddPagingArguments()
				.AddSorting()
				.AddFiltering();

			return services;
		}
	}
}
