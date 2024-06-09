using System;
using System.Threading.Tasks.Dataflow;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using GreenMileSharing.Client.Extensions;
using GreenMileSharing.Client.HttpHandlers;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.ViewModels;
using GreenMileSharing.Client.Views;
using HotAvalonia;
using Live.Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Client;

public partial class App : Application, ILiveView
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
        services.AddTransient<ApiKeyMessageHandler>();
        
        services.AddWebApiClient<IIdentityWebApi, ApiKeyMessageHandler>()
            .AddWebApiClient<IEmployeesWebApi>()
            .AddWebApiClient<ICarsWebApi>()
            .AddWebApiClient<ITripsWebApi>();
        
        Services = services.BuildServiceProvider(true);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
            desktop.MainWindow = new MainWindow();
        }
        
        base.OnFrameworkInitializationCompleted();
    }

    public object CreateView(Window window)
    {
        window.DataContext ??= Services.GetRequiredService<LoginViewModel>();
        return Services.GetRequiredService<LoginWindow>();
    }
}