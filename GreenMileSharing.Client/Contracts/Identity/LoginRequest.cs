namespace GreenMileSharing.Client.Contracts.Identity;

internal sealed class LoginRequest
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}