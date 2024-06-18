using GreenMileSharing.IdentityApi.Application.Contracts.Responses;

namespace GreenMileSharing.IdentityApi.Application.Services.Abstractions;

public interface IAuthService<in TUser>
{
    Task<AuthenticationResponse?> RegisterAsync(TUser user, string password, string role, CancellationToken cancellationToken);    
    
    Task<AuthenticationResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken);
}