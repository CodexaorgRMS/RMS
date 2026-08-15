using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Abstractions;
using Sales.Infrastructure.Data;
using Wolverine.EntityFrameworkCore;
namespace Sales.Infrastructure.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, 
			IConfiguration configuration)
		{ 
			// Register your infrastructure services here
			// Example: services.AddDbContext<YourDbContext>(options => ...);

			var connectionString = configuration.GetConnectionString("Constr");
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("Connection string 'Constr' not found in appsettings.json. Check your configuration.");
			}

			services.AddDbContextWithWolverineIntegration<SalesDbContext>((sp,
			options) =>
			{

				options.UseSqlServer(connectionString, sqlOptions =>
				{
					sqlOptions.MigrationsAssembly(typeof(SalesDbContext).Assembly.GetName().Name);
				});
			});



			services.AddScoped<ISalesDataContext>(provider => provider.GetRequiredService<SalesDbContext>());


			return services;
		}
	}
}
