using FluentResults;
using Identity.Application.Features.Auth.Events;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Wolverine;
using Wolverine.Attributes;

namespace Identity.Application.Features.Auth.VerifyEmail;

[Transactional]
public static class VerifyEmailCommandHandler
{
    public static async Task<Result> Handle(
        VerifyEmailCommand command,
        UserManager<ApplicationUser> _userManager,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null)
            return Result.Fail(new Error("Invalid OTP or Email.").WithMetadata("ErrorCode", "Auth.InvalidOtp"));

        if (user.Otp != command.Otp || user.OtpExpiration < DateTime.UtcNow)
        {
            return Result.Fail(new Error("Invalid or expired OTP.").WithMetadata("ErrorCode", "Auth.InvalidOtp"));
        }

        user.EmailConfirmed = true;
        user.Otp = null;
        user.OtpExpiration = null;
        
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Result.Fail(new Error("Failed to verify email.").WithMetadata("ErrorCode", "Auth.VerificationFailed"));
        }

        // Tell the saga the email is verified
        await bus.PublishAsync(new VerifyUserEmail(user.Id));

        return Result.Ok();
    }
}
