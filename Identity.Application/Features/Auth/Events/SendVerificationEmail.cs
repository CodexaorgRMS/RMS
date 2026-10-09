namespace Identity.Application.Features.Auth.Events
{
	public record SendVerificationEmail(Guid UserId, string Email);
}
