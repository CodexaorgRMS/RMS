using FluentResults;
using Identity.Application.Abstractions.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Identity.Infrastucture.Services;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;
	private readonly ILogger<EmailService> _logger;

	public EmailService(
		IConfiguration configuration,
		ILogger<EmailService> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	public async Task<Result> SendAsync(
		string to,
		string subject,
		string body)
	{
		var smtpServer =
			_configuration["EmailSettings:SmtpServer"];

		var portValue =
			_configuration["EmailSettings:Port"];

		var username =
			_configuration["EmailSettings:Username"];

		var password =
			_configuration["EmailSettings:Password"];

		var fromEmail =
			_configuration["EmailSettings:FromEmail"];

		if (string.IsNullOrWhiteSpace(smtpServer) ||
			!int.TryParse(portValue, out var port) ||
			port <= 0 ||
			string.IsNullOrWhiteSpace(username) ||
			string.IsNullOrWhiteSpace(password) ||
			string.IsNullOrWhiteSpace(fromEmail))
		{
			_logger.LogError(
				"Email settings are not configured properly.");

			return Result.Fail(
				"Email settings are not configured properly.");
		}

		if (string.IsNullOrWhiteSpace(to) ||
			string.IsNullOrWhiteSpace(subject) ||
			string.IsNullOrWhiteSpace(body))
		{
			_logger.LogError(
				"Email parameters cannot be null or empty.");

			return Result.Fail(
				"Email parameters cannot be null or empty.");
		}

		try
		{
			using var client = new SmtpClient();

			await client.ConnectAsync(
				smtpServer,
				port,
				SecureSocketOptions.StartTls);

			await client.AuthenticateAsync(
				username,
				password);

			var message = new MimeMessage();

			message.From.Add(
				MailboxAddress.Parse(fromEmail));

			message.To.Add(
				MailboxAddress.Parse(to));

			message.Subject = subject;

			var builder = new BodyBuilder
			{
				TextBody =
					"Your email client does not support HTML. " +
					"Please use a modern email client.",

				HtmlBody = body
			};

			message.Body = builder.ToMessageBody();

			await client.SendAsync(message);

			_logger.LogInformation(
				"Email sent successfully to {To} with subject {Subject}",
				to,
				subject);

			await client.DisconnectAsync(true);

			return Result.Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError(
				ex,
				"Failed to send email to {To}",
				to);

			return Result.Fail(
				$"Failed to send email: {ex.Message}");
		}
	}

	public Task SendOtpAsync(
		string email,
		string otp)
	{
		return SendAsync(
			email,
			"Your OTP Code",
			$"Your OTP is: {otp}");
	}
}