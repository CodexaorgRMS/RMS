namespace Identity.Application.Features.Auth.Common;

public record AuthResponse(string AccessToken, DateTime Expiration);
