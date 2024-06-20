using Asp.Versioning;
using GreenMileSharing.IdentityApi.Application.Contracts.Requests;
using GreenMileSharing.IdentityApi.Application.Helpers;
using GreenMileSharing.IdentityApi.Application.Mappers;
using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.IdentityApi.Application.Services;
using GreenMileSharing.IdentityApi.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GreenMileSharing.IdentityApi.Controllers.V1;

[ApiVersion(1.0)]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService<ApplicationUser> _authService;

    public AuthController([FromKeyedServices(nameof(JsonAuthService))]IAuthService<ApplicationUser> authService)
    {
        _authService = authService;
    }

    [HttpPost(ApiEndpoints.Authentication.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var applicationUser = request.ToUser();
        var authResponse = await _authService.RegisterAsync(applicationUser, request.Password, request.Role, cancellationToken);
        
        return authResponse is not null
            ? Ok(authResponse)
            : BadRequest();
    }

    [HttpPost(ApiEndpoints.Authentication.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResponse = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
        return authResponse is not null
            ? Ok(authResponse)
            : BadRequest("Login credentials are incorrect");
    }
}