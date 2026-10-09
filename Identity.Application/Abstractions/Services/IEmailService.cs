using FluentResults;

namespace Identity.Application.Abstractions.Services
{
	public interface IEmailService
	{
		Task<Result> SendAsync(string to, string subject, string body);
		Task SendOtpAsync(string email, string otp);
	}
}
