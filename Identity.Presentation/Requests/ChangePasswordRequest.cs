using FluentValidation;

namespace Identity.Presentation.Requests
{
	public record ChangePasswordRequest(string Email, string OldPassword, string NewPassword);


	public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
	{
		public ChangePasswordRequestValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.OldPassword).NotEmpty();
			RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
		}
	}
}
