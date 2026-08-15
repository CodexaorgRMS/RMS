using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using Sales.Presentation.Mapping;
using Sales.Presentation.Requests.AddItem;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sales.Presentation.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddSalesPresentationServices(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<AddOrderItemRequestValidator>(ServiceLifetime.Singleton);

			services.Scan(scan => scan
			   .FromAssembliesOf(typeof(OrderMapper))
			   .AddClasses(classes => classes
				   .Where(type => type.GetCustomAttribute<MapperAttribute>() != null))
			   .AsSelf()
			   .WithSingletonLifetime());

		
			return services;
		}
	}
}
