using Identity.Application.Features.Auth.ChangePassword;
using Identity.Application.Features.Auth.ForgetPassword;
using Identity.Application.Features.Auth.Login;
using Identity.Application.Features.Auth.ResetPassword;
using Identity.Application.Features.Auth.VerifyEmail;
using Identity.Presentation.Requests;
using Riok.Mapperly.Abstractions;

namespace Identity.Presentation.Mapping
{
	[Mapper]
	public partial class AuthMapper
	{
		public partial LoginCommand MapLogin(LoginRequest request);
		public partial ForgetPasswordCommand MapForgetPassword(ForgetPasswordRequest request);
		public partial ResetPasswordCommand MapResetPassword(ResetPasswordRequest request);
		public partial ChangePasswordCommand MapChangePassword(ChangePasswordRequest request);
		public partial VerifyEmailCommand MapVerifyEmail(VerifyUserEmailRequest request);
	}
}
