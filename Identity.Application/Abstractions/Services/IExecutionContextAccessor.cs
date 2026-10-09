using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Abstractions.Services
{
	public interface IExecutionContextAccessor
	{
		HttpContext HttpContext { get; }
		Guid UserId { get; }
		string UserName { get; }
		Guid CorrelationId { get; }


		bool IsAvailable { get; }
		string? IpAddress { get; }
	}
}
