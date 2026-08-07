using Inventory.Presentation.DependancyInjection;
using SharedPresentation.Common;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var modules = new List<IModule> { new InventoryModule()};

builder.Services.AddModules(builder.Configuration, modules);

builder.Host.UseWolverine(opts =>
{
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

app.MapWolverineEndpoints(); 

app.Run();