namespace GreenMileSharing.Client.Contracts.Identity;

internal sealed class RegisterRequest
{
    public string UserName { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;
}