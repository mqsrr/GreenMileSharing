﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace GreenMileSharing.IdentityApi.Application.Services.Abstractions;

public interface ITokenWriter<in TEntity>
    where TEntity : IdentityUser
{
    Task<string> WriteTokenAsync(TEntity entity, CancellationToken cancellationToken);

    Task<string> WriteTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
}