namespace GreenMileSharing.IdentityApi.Application.Contracts.Requests;

public sealed class RegisterRequest
{
    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string Role { get; init; } = "User";
}