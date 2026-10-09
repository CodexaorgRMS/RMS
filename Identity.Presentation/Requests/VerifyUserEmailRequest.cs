namespace Identity.Presentation.Requests
{
	public record VerifyUserEmailRequest(string Email, string OTP);

}
