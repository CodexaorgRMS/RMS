using Identity.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using SharedInfrastructure.Middlewares;
using System.Security.Claims;

namespace Identity.Infrastucture.Services
{
	public sealed class ExecutionContextAccessor : IExecutionContextAccessor
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public HttpContext HttpContext => _httpContextAccessor.HttpContext!;

		public bool IsAvailable => _httpContextAccessor.HttpContext is not null;

		public Guid UserId
		{
			get
			{
				var value = _httpContextAccessor.HttpContext?
					.User
					.FindFirstValue(ClaimTypes.NameIdentifier);

				return Guid.TryParse(value, out var userId)
					? userId
					: Guid.Empty;
			}
		}

		public string? UserName =>
			_httpContextAccessor.HttpContext?
				.User
				.Identity?
				.Name;

		public Guid CorrelationId
		{
			get
			{
				if (!IsAvailable)
					return Guid.Empty;

				var headers = _httpContextAccessor.HttpContext!.Request.Headers;

				if (!headers.ContainsKey(CorrelationMiddleware.CorrelationIdHeaderkey))
					return Guid.Empty;

				return Guid.TryParse(
					headers[CorrelationMiddleware.CorrelationIdHeaderkey],
					out var correlationId)
					? correlationId
					: Guid.Empty;
			}
		}


		public string? IpAddress =>
		_httpContextAccessor.HttpContext?
			.Connection
			.RemoteIpAddress?
			.ToString();
	}
}
