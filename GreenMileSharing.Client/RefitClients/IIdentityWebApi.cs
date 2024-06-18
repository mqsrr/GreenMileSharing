using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Contracts.Identity;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

internal interface IIdentityWebApi
{
    [Post(ApiEndpoints.Authentication.Register)]
    Task<IApiResponse<AuthResponse>> RegisterAsync(string apiVersion, [Body] RegisterRequest request, CancellationToken cancellationToken);

    [Post(ApiEndpoints.Authentication.Login)]
    Task<IApiResponse<AuthResponse>> LoginAsync(string apiVersion, [Body] LoginRequest request, CancellationToken cancellationToken);
}