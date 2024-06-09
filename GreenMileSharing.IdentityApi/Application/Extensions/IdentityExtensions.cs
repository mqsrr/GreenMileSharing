using GreenMileSharing.IdentityApi.Application.Models;
using GreenMileSharing.IdentityApi.Persistence;
using Microsoft.AspNetCore.Identity;

namespace GreenMileSharing.IdentityApi.Application.Extensions;

internal static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
        PasswordOptions? passwordOptions = null, UserOptions? userOptions = null)
    {
        passwordOptions ??= new PasswordOptions
        {
            RequireDigit = true,
            RequiredLength = 6,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = true
        };

        userOptions ??= new UserOptions
        {
            RequireUniqueEmail = true
        };

        services.AddIdentity<ApplicationUser, IdentityRole>(x =>
            {
                x.Password = passwordOptions;
                x.User = userOptions;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}