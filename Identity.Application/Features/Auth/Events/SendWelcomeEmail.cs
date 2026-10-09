namespace Identity.Application.Features.Auth.Events
{
	public record SendWelcomeEmail(Guid UserId, string Email, string FirstName);
}
