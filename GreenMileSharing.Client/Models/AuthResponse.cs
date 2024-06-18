namespace GreenMileSharing.Client.Models;

internal sealed class AuthResponse
{
    public required string Token { get; init; }
    
    public required string Role { get; init; }
}