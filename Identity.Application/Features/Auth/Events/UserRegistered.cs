namespace Identity.Application.Features.Auth.Events
{
	public record UserRegistered(Guid UserId, string Email, string UserName);
}
