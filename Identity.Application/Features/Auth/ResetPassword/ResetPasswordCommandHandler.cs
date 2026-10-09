using FluentResults;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Wolverine.Attributes;

namespace Identity.Application.Features.Auth.ResetPassword;

[Transactional]
public static class ResetPasswordCommandHandler
{

    public static async Task<Result> Handle(ResetPasswordCommand command,
		UserManager<ApplicationUser> _userManager,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null)
            return Result.Fail(new Error("Invalid OTP or Email.").WithMetadata("ErrorCode", "Auth.InvalidOtp"));

        if (user.Otp != command.Otp || user.OtpExpiration < DateTime.UtcNow)
        {
            return Result.Fail(new Error("Invalid or expired OTP.").WithMetadata("ErrorCode", "Auth.InvalidOtp"));
        }

        // Reset password using Identity's token system under the hood
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, command.NewPassword);

        if (!result.Succeeded)
        {
            return Result.Fail(new Error("Failed to reset password.").WithMetadata("ErrorCode", "Auth.ResetFailed"));
        }

        // Clear the OTP to prevent reuse
        user.Otp = null;
        user.OtpExpiration = null;
        await _userManager.UpdateAsync(user);

        return Result.Ok();
    }
}
