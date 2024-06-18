namespace GreenMileSharing.IdentityApi.Application.Models;

internal sealed class IdentityRole
{
    public required string IdentityId { get; init; }

    public required string Role { get; init; }
}