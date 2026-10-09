namespace Identity.Application.Features.Auth.ChangePassword;

public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword);
