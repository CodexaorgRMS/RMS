namespace Identity.Application.Features.Auth.ResetPassword;

public record ResetPasswordCommand(string Email, string Otp, string NewPassword);
