
using FluentResults;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Wolverine.Attributes;

namespace Identity.Application.Features.Auth.ForgetPassword;

[Transactional]
public static class ForgetPasswordCommandHandler
{

    public static async Task<Result> Handle(ForgetPasswordCommand command,
		UserManager<ApplicationUser> _userManager,
        Application.Abstractions.Services.IEmailService _emailService,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null)
            return Result.Ok(); // Do not reveal that the user does not exist for security reasons

        // Generate 6-digit OTP
        var random = new Random();
        var otp = random.Next(100000, 999999).ToString();

        user.Otp = otp;
        user.OtpExpiration = DateTime.UtcNow.AddMinutes(15);
        await _userManager.UpdateAsync(user);

        await _emailService.SendOtpAsync(user.Email, otp);

        return Result.Ok();
    }
}
