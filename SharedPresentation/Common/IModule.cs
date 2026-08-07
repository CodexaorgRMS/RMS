using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedPresentation.Common
{
	public interface IModule
	{
		string Name { get; }

		IServiceCollection RegisterModule(
			IServiceCollection services,
			IConfiguration configuration);

		IApplicationBuilder UseModule(
			IApplicationBuilder app);

		Assembly GetPresentationAssembly();
		Assembly GetApplicationAssembly();
	}
}
