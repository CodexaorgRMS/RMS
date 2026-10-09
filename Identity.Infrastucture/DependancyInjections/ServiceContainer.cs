using Identity.Application.Abstractions.Shared;
using Identity.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.EntityFrameworkCore;
namespace Identity.Infrastucture.DependancyInjections
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddIdntityInfrastructure(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Constr");
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("Connection string 'Constr' not found in configuration.");
			}

			services.AddDbContextWithWolverineIntegration<AppIdentityDbContext>((sp, options) =>
			{
				options.UseSqlServer(connectionString, sqlOptions =>
				{
					sqlOptions.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.GetName().Name);
				});
			});

			services.AddScoped<IIdentityDataContext>(sp => sp.GetRequiredService<AppIdentityDbContext>());

			return services;
		}
	}

}
