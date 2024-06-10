using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GreenMileSharing.Client.Extensions;
using GreenMileSharing.Client.HttpHandlers;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.Views;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Client;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        this.EnableHotReload();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        services.RegisterViewModels();
        services.RegisterViews();
        services.AddMemoryCache();
        
        services.AddTransient<BearerAuthorizationMessageHandler>();
        services.AddTransient<ApiKeyAuthorizationMessageHandler>();
        
        services.AddWebApiClient<IIdentityWebApi, ApiKeyAuthorizationMessageHandler>()
            .AddWebApiClient<IEmployeesWebApi>()
            .AddWebApiClient<ICarsWebApi>()
            .AddWebApiClient<ITripsWebApi>();
        
        Services = services.BuildServiceProvider(true);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
            desktop.MainWindow = new LoginWindow();
        }
        
        base.OnFrameworkInitializationCompleted();
    }
}