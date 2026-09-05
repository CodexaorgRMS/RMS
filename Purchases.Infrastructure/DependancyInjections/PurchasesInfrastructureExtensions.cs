using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchases.Application.Abstractions;
using Purchases.Application.Features.Purchases.Commands.Create;
using Purchases.Application.Features.Purchases.Commands.Receive;
using Purchases.Application.Features.Purchases.Commands.Submit;
using Purchases.Application.Features.Purchases.Commands.Update;
using Purchases.Application.Features.Suppliers.Commands.CreateSupplier;
using Purchases.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.EntityFrameworkCore;

namespace Purchases.Infrastructure.DependancyInjections
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddPurchasesInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("constr");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'Constr' not found in appsettings.json. Check your configuration.");
            }

            services.AddDbContextWithWolverineIntegration<PurchasesDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(PurchasesDbContext).Assembly.GetName().Name);
                });
            });

            services.AddScoped<IPurchasesDataContext>(provider => provider.GetRequiredService<PurchasesDbContext>());
            services.AddScoped<IValidator<CreatePurchaseCommand>, CreatePurchaseCommandValidator>();
            services.AddScoped<IValidator<UpdatePurchaseCommand>, UpdatePurchaseCommandValidator>();
            services.AddScoped<IValidator<ReceivePurchaseCommand>, ReceivePurchaseCommandValidator>();
            services.AddSingleton<IValidator<CreateSupplierCommand>, CreateSupplierValidator>();
            services.AddSingleton<IValidator<SubmitPurchaseCommand>, SubmitPurchaseCommandValidator>();

            return services;
        }
    }
}
