namespace GreenMileSharing.IdentityApi.Application.Contracts.Responses;

public sealed class AuthenticationResponse
{
    public required string Role { get; init; }
    
    public required string Token { get; init; }
}