using GreenMileSharing.Client.ViewModels;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Client.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection RegisterViews(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Scan(selector =>
            selector.FromAssemblyOf<MainWindow>()
                .AddClasses()
                .AsSelf()
                .WithLifetime(lifetime));

        return services;
    }
    
    internal static IServiceCollection RegisterViewModels(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.Scan(selector =>
            selector.FromAssemblyOf<MainWindowViewModel>()
                .AddClasses(filter => filter.Where(type => !type.IsPublic))
                .AsSelf()
                .WithLifetime(lifetime));

        return services;
    }
}