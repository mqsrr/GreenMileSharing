namespace GreenMileSharing.IdentityApi.Application.Contracts.Responses;

public sealed class AuthenticationResponse
{
    public required string Token { get; init; }
}