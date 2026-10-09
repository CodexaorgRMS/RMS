using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace SharedInfrastructure.Middlewares
{
	public class CorrelationMiddleware
	{
		public const string CorrelationIdHeaderkey = "X-Correlation-ID";
		private readonly RequestDelegate _next;

		public CorrelationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{

			string correlationId = context.Request.Headers.ContainsKey(CorrelationIdHeaderkey)
				? context.Request.Headers[CorrelationIdHeaderkey].ToString()
				: Guid.NewGuid().ToString();

			context.Items[CorrelationIdHeaderkey] = correlationId;

			context.Response.OnStarting(() =>
			{
				context.Response.Headers[CorrelationIdHeaderkey] = correlationId;
				return Task.CompletedTask;
			});

			using (LogContext.PushProperty("CorrelationId", correlationId))
			{
				await _next(context);
			}
		}
	}
}
