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

}