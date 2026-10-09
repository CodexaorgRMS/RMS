using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Presentation.Requests
{
	public record ForgetPasswordRequest(string Email);


	public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
	{
		public ForgetPasswordRequestValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
		}
	}
}
