using Identity.Application.Features.Auth.Events;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace Identity.Application.Features.Auth.Sagas
{
	public class UserOnboardingSaga : Saga
	{
		public Guid Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public bool IsVerificationEmailSent { get; set; }
		public bool IsEmailVerified { get; set; }
		public bool IsWelcomeEmailSent { get; set; }
		public DateTime StartedAt { get; set; }



		// Step 1: Start the saga when UserRegistered is published


		public static (UserOnboardingSaga, SendVerificationEmail, OnboardingTimedOut)
			Start(UserRegistered @event)
		{


			var saga = new UserOnboardingSaga
			{
				Id = @event.UserId,
				Email = @event.Email,
				UserName = @event.UserName,
				StartedAt = DateTime.UtcNow
			};


			var sendVerificationEmail = new SendVerificationEmail(@event.UserId,
				@event.Email);

			var timeoutMessage = new OnboardingTimedOut(@event.UserId);


			return (saga, sendVerificationEmail, timeoutMessage);
		}


		// Step 2: Verification email was sent
		public void Handle(
			VerificationEmailSent @event,
			ILogger<UserOnboardingSaga> logger)
		{
			logger.LogInformation(
				"Verification email sent for user {UserId}", Id);

			IsVerificationEmailSent = true;
		}

		// Step 3: User verified their email
		public SendWelcomeEmail Handle(
			VerifyUserEmail command,
			ILogger<UserOnboardingSaga> logger)
		{
			logger.LogInformation("Email verified for user {UserId}", Id);

			IsEmailVerified = true;

			return new SendWelcomeEmail(Id, Email, UserName);
		}


		// Step 4: Welcome email sent - onboarding complete
		public void Handle(
			WelcomeEmailSent @event,
			ILogger<UserOnboardingSaga> logger)
		{
			logger.LogInformation("Onboarding complete for user {UserId}", Id);

			IsWelcomeEmailSent = true;

			MarkCompleted();
		}


		// Compensation: timeout handler
		public DeleteUnverifiedUser? Handle(
			OnboardingTimedOut timeout,
			ILogger<UserOnboardingSaga> logger)
		{
			if (IsEmailVerified)
			{
				logger.LogInformation(
					"Timeout ignored - email already verified for user {UserId}",
					Id);
				return null;
			}

			logger.LogWarning(
				"Onboarding timed out for user {UserId} - email not verified",
				Id);

			MarkCompleted();


			return new DeleteUnverifiedUser(UserId: Id);
		}

		// NotFound: messages arriving for completed/deleted sagas
		public static void NotFound(
			VerifyUserEmail command,
			ILogger<UserOnboardingSaga> logger)
		{
			logger.LogWarning(
				"Verify email received but saga {Id} no longer exists",
				command.Id);
		}

		public static void NotFound(
	OnboardingTimedOut timeout,
	ILogger<UserOnboardingSaga> logger)
		{
			logger.LogInformation(
				"Timeout received for already-completed saga {Id}",
				timeout.Id);
		}


	}
}
