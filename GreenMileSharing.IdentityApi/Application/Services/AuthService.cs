using GreenMileSharing.IdentityApi.Application.Contracts.Responses;
using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.IdentityApi.Application.Services.Abstractions;
using GreenMileSharing.Messages;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;

namespace GreenMileSharing.IdentityApi.Application.Services;

internal sealed class AuthService : IAuthService<ApplicationUser>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenWriter<ApplicationUser> _tokenWriter;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISendEndpointProvider _endpointProvider;

    public AuthService(SignInManager<ApplicationUser> signInManager, ITokenWriter<ApplicationUser> tokenWriter, ISendEndpointProvider endpointProvider, RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _tokenWriter = tokenWriter;
        _endpointProvider = endpointProvider;
        _roleManager = roleManager;
    }

    public async Task<AuthenticationResponse?> RegisterAsync(ApplicationUser user, string password, string role, CancellationToken cancellationToken)
    {
        
        var result = await _signInManager.UserManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return null;
        }

        var registerResult = await _signInManager.PasswordSignInAsync(user.UserName!, password, false, false);
        if (!registerResult.Succeeded)
        {
            return null;
        }

        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole("Manager"));
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
        
        var claims = await _signInManager.CreateUserPrincipalAsync(user);
        await _signInManager.UserManager.AddClaimsAsync(user, claims.Claims);
        await _signInManager.UserManager.AddToRoleAsync(user, role);
        
        string token = await _tokenWriter.WriteTokenAsync(user, cancellationToken);
        await _endpointProvider.Send<RegisterEmployee>(new
        {
            Id = user.Id,
            Username = user.UserName
        }, cancellationToken).ConfigureAwait(false);
        
        return new AuthenticationResponse
        {
            Token = token,
            Role = role
        };
    }


    public async Task<AuthenticationResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
        if (!signInResult.Succeeded)
        {
            return null;
        }
        
        var applicationUser = await _signInManager.UserManager.FindByNameAsync(username);
        string token = await _tokenWriter.WriteTokenAsync(applicationUser!, cancellationToken);
        
        var currentRoles = await _signInManager.UserManager.GetRolesAsync(applicationUser!);
        return new AuthenticationResponse
        {
            Token = token,
            Role = currentRoles.First()
        };
    }
}