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
            .AddClasses(classes => classes.AssignableTo<TInterface>())
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddApplicationServiceAsSelf<TImplementation>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TImplementation>()
            .AddClasses(classes => classes.AssignableTo<TImplementation>())
            .AsSelfWithInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }

    public static IServiceCollection AddApplicationService(
        this IServiceCollection services,
        Type interfaceType,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(interfaceType)
            .AddClasses(classes => classes.AssignableTo(interfaceType))
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }
    
}