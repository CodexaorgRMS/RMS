using Customers.Application.Abstractions;
using Customers.Infrastructure.Data;
using Customers.Presentation.DependancyInjections;
using Finance.Application.Abstractions;
using Finance.Infrastructure.Data;
using Finance.Presentation.DependancyInjections;
using Inventory.Application.Abstractions;
using Inventory.Infrastructure.Data;
using Inventory.Presentation.DependancyInjection;
using JasperFx.CodeGeneration.Model;
using Offers.Application.Abstractions;
using Offers.Infrastructure.Data;
using Offers.Presentation.DependancyInjection;
using Sales.Application.Abstractions;
using Sales.Infrastructure.Data;
using Sales.Presentation.DependancyInjections;
using SharedInfrastructure.DependancyInjections;
using SharedInfrastructure.ExeptionHandling;
using SharedPresentation.Common;
using SharedPresentation.ExceptionHandling;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.SqlServer;
using SharedInfrastructure.DependancyInjections;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSharedInfrastructure();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var modules = new List<IModule> { new InventoryModule(), new SalesModule(), new CustomersModule(), new OffersModule(), new FinanceModule() };

builder.Services.AddModules(builder.Configuration, modules);

// FluentValidation
foreach (var module in modules)
{
	var presentationAssembly = module.GetPresentationAssembly();
	var applicationAssembly = module.GetApplicationAssembly();
	builder.Services.AddValidatorsFromAssembly(presentationAssembly);
	builder.Services.AddValidatorsFromAssembly(applicationAssembly);
}

var connectionString = builder.Configuration.GetConnectionString("Constr");


builder.Host.UseWolverine(opts =>
{
	opts.UseRuntimeCompilation();

	opts.UseFluentValidation();

	opts.UseEntityFrameworkCoreTransactions()
	.WithDbContextAbstraction<IInventoryDataContext, InventoryDbContext>()
	.WithDbContextAbstraction<ISalesDataContext, SalesDbContext>()
	.WithDbContextAbstraction<ICustomersDataContext, CustomersDbContext>()
	.WithDbContextAbstraction<IOffersDataContext, OffersDbContext>()
	.WithDbContextAbstraction<IFinanceDataContext, FinanceDbContext>();


	opts.PersistMessagesWithSqlServer(connectionString!, "wolverine");


	opts.Policies.UseDurableLocalQueues();

	//	opts.Services.AddResourceSetupOnStartup();


	opts.ServiceLocationPolicy = ServiceLocationPolicy.AllowedButWarn;


	opts.UseSystemTextJsonForSerialization();

	foreach (var module in modules)
	{
		opts.Discovery.IncludeAssembly(module.GetPresentationAssembly());
		opts.Discovery.IncludeAssembly(module.GetApplicationAssembly());
	}

});

builder.Services.AddWolverineHttp();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Market API V1");

        options.RoutePrefix = "swagger";
    });
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseModules();

app.MapWolverineEndpoints(opts =>
{
	opts.UseFluentValidationProblemDetailMiddleware();
});

app.Run();