using FluentResults;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Wolverine.Attributes;

namespace Identity.Application.Features.Auth.ChangePassword;

[Transactional]
public static class ChangePasswordCommandHandler
{
    public static async Task<Result> Handle(ChangePasswordCommand command,
		UserManager<ApplicationUser> _userManager,
		CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null)
            return Result.Fail(new Error("User not found.").WithMetadata("ErrorCode", "Auth.UserNotFound"));

        var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);

        if (!result.Succeeded)
        {
            return Result.Fail(new Error("Failed to change password. Ensure old password is correct.")
                .WithMetadata("ErrorCode", "Auth.ChangePasswordFailed"));
        }

        return Result.Ok();
    }
}
