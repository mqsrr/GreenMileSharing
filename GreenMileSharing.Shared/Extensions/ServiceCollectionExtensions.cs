using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationService<TInterface>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TInterface>()
            .AddClasses(classes => classes.AssignableTo<TInterface>().Where(type => !type.Name.Contains("Json")))
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddUrlApiVersioning(
        this IServiceCollection services,
        Action<ApiVersioningOptions>? versioningOptions = null,
        Action<ApiExplorerOptions>? explorerOptions = null)
    {
        versioningOptions ??= options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(2.0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        };

        explorerOptions ??= options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        };
        
        services.AddApiVersioning(versioningOptions)
            .AddMvc()
            .AddApiExplorer(explorerOptions);
        return services;
    }
}