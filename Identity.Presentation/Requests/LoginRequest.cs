using FluentValidation;

namespace Identity.Presentation.Requests
{
	public record LoginRequest(string Email, string Password);


	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
