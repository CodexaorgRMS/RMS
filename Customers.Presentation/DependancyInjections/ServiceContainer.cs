using Customers.Application.Features.Customers.Commands.CreateCustomer;
using Customers.Presentation.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using System.Reflection;

namespace Customers.Presentation.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddCustomersPresentationServices(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>(ServiceLifetime.Singleton);

			services.Scan(scan => scan
			   .FromAssembliesOf(typeof(CustomerMapper))
			   .AddClasses(classes => classes
				   .Where(type => type.GetCustomAttribute<MapperAttribute>() != null))
			   .AsSelf()
			   .WithSingletonLifetime());

			return services;
		}
	}
}
