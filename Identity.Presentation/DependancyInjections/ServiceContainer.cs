using FluentValidation;
using Identity.Presentation.Mapping;
using Identity.Presentation.Requests;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using System.Reflection;

namespace Identity.Presentation.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddIdentityPresentationServices(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>(ServiceLifetime.Singleton);

			services.Scan(scan => scan
			   .FromAssembliesOf(typeof(AuthMapper))
			   .AddClasses(classes => classes
				   .Where(type => type.GetCustomAttribute<MapperAttribute>() != null))
			   .AsSelf()
			   .WithSingletonLifetime());

			return services;
		}
	}
}
