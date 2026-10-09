using FluentValidation;

namespace Identity.Presentation.Requests
{
	public record ResetPasswordRequest(string Email, string Otp, string NewPassword);


	public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Otp).NotEmpty().Length(6);
			RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
		}
	}

}
