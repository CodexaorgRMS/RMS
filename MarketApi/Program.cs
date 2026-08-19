using Customers.Application.Abstractions;
using Customers.Infrastructure.Data;
using Customers.Presentation.DependancyInjections;
using Inventory.Application.Abstractions;
using Inventory.Infrastructure.Data;
using Inventory.Presentation.DependancyInjection;
using JasperFx.CodeGeneration.Model;
using Purchases.Application.Abstractions;
using Purchases.Infrastructure.Data;
using Purchases.Presentation.DependancyInjections;
using Sales.Application.Abstractions;
using Sales.Infrastructure.Data;
using Sales.Presentation.DependancyInjections;
using SharedInfrastructure.DependancyInjections;
using SharedInfrastructure.ExeptionHandling;
using SharedPresentation.Common;
using SharedPresentation.GraphQL;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.SqlServer;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSharedInfrastructure();
builder.Services.AddSharedGraphQLServices();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var modules = new List<IModule> {
		new InventoryModule(),
		new SalesModule(),
		new CustomersModule(),
		new PurchasesModule(),
        };

builder.Services.AddModules(builder.Configuration, modules);


var connectionString = builder.Configuration.GetConnectionString("Constr");


builder.Host.UseWolverine(opts =>
{
	opts.UseRuntimeCompilation();

	opts.UseFluentValidation();

	opts.UseEntityFrameworkCoreTransactions()
	.WithDbContextAbstraction<IInventoryDataContext, InventoryDbContext>()
	.WithDbContextAbstraction<ISalesDataContext, SalesDbContext>()
	.WithDbContextAbstraction<ICustomersDataContext, CustomersDbContext>()
	.WithDbContextAbstraction<IPurchasesDataContext, PurchasesDbContext>();

	opts.PersistMessagesWithSqlServer(connectionString!, "wolverine");


	opts.Policies.UseDurableLocalQueues();

	opts.AutoBuildMessageStorageOnStartup = JasperFx.AutoCreate.None;


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

app.UseExceptionHandler();

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

app.MapGraphQL();

app.Run();