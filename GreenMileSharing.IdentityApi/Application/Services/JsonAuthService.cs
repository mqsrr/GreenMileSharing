using GreenMileSharing.IdentityApi.Application.Contracts.Responses;
using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.IdentityApi.Application.Services.Abstractions;
using GreenMileSharing.Messages.Json;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using IdentityRole = GreenMileSharing.IdentityApi.Application.Models.IdentityRole;

namespace GreenMileSharing.IdentityApi.Application.Services;

internal sealed class JsonAuthService : IAuthService<ApplicationUser>
{
    private readonly ITokenWriter<ApplicationUser> _tokenWriter;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    
    public JsonAuthService(ITokenWriter<ApplicationUser> tokenWriter, ISendEndpointProvider sendEndpointProvider)
    {
        _tokenWriter = tokenWriter;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task<AuthenticationResponse?> RegisterAsync(ApplicationUser user, string password, string role,CancellationToken cancellationToken)
    {
        if (!File.Exists("identities.json"))
        {
            File.Create("identities.json").Close();
        }
        
        if (!File.Exists("roles.json"))
        {
            File.Create("roles.json").Close();
        }
        string identitiesJson = await File.ReadAllTextAsync("identities.json", cancellationToken);
        string identityRolesJson = await File.ReadAllTextAsync("roles.json", cancellationToken);
        
        var identities = JsonConvert.DeserializeObject<List<ApplicationUser>>(identitiesJson) ?? [];
        var identityRoles = JsonConvert.DeserializeObject<List<IdentityRole>>(identityRolesJson) ?? [];
        
        if (identities.Exists(identity => identity.UserName == user.UserName || identity.Email == user.Email))
        {
            return null;
        }

        user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, password);
        
        identities.Add(user);
        identityRoles.Add(new IdentityRole
        {
            IdentityId = user.Id,
            Role = role
        });
        
        await File.WriteAllTextAsync("identities.json", JsonConvert.SerializeObject(identities), cancellationToken);
        await File.WriteAllTextAsync("roles.json", JsonConvert.SerializeObject(identityRoles), cancellationToken);
        
        string token = await _tokenWriter.WriteTokenAsync(user, cancellationToken);

        await _sendEndpointProvider.Send<RegisterEmployeeJson>(new
        {
            user.Id,
            Username = user.UserName
        }, cancellationToken);
        return new AuthenticationResponse
        {
            Token = token,
            Role = role
        };
    }

    public async Task<AuthenticationResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        if (!File.Exists("identities.json"))
        {
            File.Create("identities.json").Close();
        }
        
        if (!File.Exists("identities.json"))
        {
            File.Create("identities.json").Close();
        }
        
        string identitiesJson = await File.ReadAllTextAsync("identities.json", cancellationToken);
        string rolesJson = await File.ReadAllTextAsync("roles.json", cancellationToken);
        
        var identities = JsonConvert.DeserializeObject<List<ApplicationUser>>(identitiesJson) ?? [];
        var identityRoles = JsonConvert.DeserializeObject<List<IdentityRole>>(rolesJson) ?? [];
        
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var identity = identities.Find(identity => identity.UserName == username);
        
        if (identity is null || passwordHasher.VerifyHashedPassword(identity, identity.PasswordHash!,password) is PasswordVerificationResult.Failed)
        {
            return null;
        }
        
        string token = await _tokenWriter.WriteTokenAsync(identity, cancellationToken);
        return new AuthenticationResponse
        {
            Token = token,
            Role = identityRoles.First(role => role.IdentityId == identity.Id).Role
        };
    }
}