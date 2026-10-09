using JasperFx.Core;
using Wolverine;

namespace Identity.Application.Features.Auth.Events
{
	public record OnboardingTimedOut(Guid Id) : TimeoutMessage(5.Hours());
}
