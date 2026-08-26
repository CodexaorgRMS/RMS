using Finance.Application.Abstractions;
using Finance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.EntityFrameworkCore;

namespace Finance.Infrastructure.DependancyInjections;

public static class ServiceContainer
{
    public static IServiceCollection AddFinanceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Constr");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Constr' not found in configuration.");
        }

        services.AddDbContextWithWolverineIntegration<FinanceDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(FinanceDbContext).Assembly.GetName().Name);
            });
        });

        services.AddScoped<IFinanceDataContext>(sp => sp.GetRequiredService<FinanceDbContext>());

        return services;
    }
}
