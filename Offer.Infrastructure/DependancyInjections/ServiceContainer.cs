using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offers.Application.Abstractions;
using Offers.Application.Services;
using Offers.Domain.Interfaces;
using Offers.Domain.Strategies;
using Offers.Infrastructure.Data;
using Wolverine.EntityFrameworkCore;

namespace Offers.Infrastructure.DependancyInjections;

public static class ServiceContainer
{
    public static IServiceCollection AddOffersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Constr");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Constr' not found in appsettings.json. Check your configuration.");
        }

        services.AddDbContextWithWolverineIntegration<OffersDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(OffersDbContext).Assembly.GetName().Name);
            });
        });

        services.AddScoped<IOffersDataContext>(provider => provider.GetRequiredService<OffersDbContext>());

        services.AddSingleton<IOfferStrategy, PercentageDiscountStrategy>();
        services.AddSingleton<IOfferStrategy, FixedDiscountStrategy>();
        services.AddSingleton<IOfferStrategy, BogoDiscountStrategy>();
        services.AddSingleton<IOfferStrategy, BundleDiscountStrategy>();

        services.AddScoped<IOfferEngine, OfferEngine>();

        return services;
    }
}