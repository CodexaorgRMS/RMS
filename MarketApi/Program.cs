using FluentValidation;
using Inventory.Presentation.DependancyInjection;
using JasperFx.CodeGeneration.Model;
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

var modules = new List<IModule> { new InventoryModule()};

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

	opts.UseEntityFrameworkCoreTransactions();

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

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseModules();

app.MapWolverineEndpoints(opts =>
{
	opts.UseFluentValidationProblemDetailMiddleware();
});

app.Run();