// ModuleExtensions.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedPresentation.Common;

public static class ModuleExtensions
{
	public static IServiceCollection AddModules(
		this IServiceCollection services,
		IConfiguration configuration,
		IEnumerable<IModule> modules)
	{
		var moduleList = modules.ToList();

		foreach (var module in moduleList)
		{
			module.RegisterModule(services, configuration);
		}

		services.AddSingleton<IReadOnlyList<IModule>>(moduleList);
		return services;
	}

	public static WebApplication UseModules(this WebApplication app)
	{
		var modules = app.Services.GetRequiredService<IReadOnlyList<IModule>>();
		foreach (var module in modules)
		{
			module.UseModule(app);
		}
		return app;
	}
}