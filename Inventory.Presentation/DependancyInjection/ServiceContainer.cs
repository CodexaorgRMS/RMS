using FluentValidation;
using Inventory.Presentation.Categories.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Presentation.DependancyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInventoryPresentationServices(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();



			return services;
		}
	}
}
