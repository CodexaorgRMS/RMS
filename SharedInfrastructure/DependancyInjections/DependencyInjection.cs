using Microsoft.Extensions.DependencyInjection;
using SharedInfrastructure.Validations;
using Wolverine.FluentValidation;

namespace SharedInfrastructure.DependancyInjections
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
		{
	
			services.AddSingleton(typeof(IFailureAction<>), typeof(ResultFailureAction<>));

			return services;
		}
	}
}
