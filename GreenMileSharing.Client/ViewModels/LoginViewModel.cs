using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Identity;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.Caching.Memory;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private LoginRequest _loginRequest;

    private readonly IIdentityWebApi _identityWebApi;
    private readonly IEmployeesWebApi _employeesWebApi;
    private readonly IMemoryCache _memoryCache;

    public LoginViewModel(IIdentityWebApi identityWebApi, IEmployeesWebApi employeesWebApi, IMemoryCache memoryCache)
    {
        _identityWebApi = identityWebApi;
        _employeesWebApi = employeesWebApi;
        _memoryCache = memoryCache;

        LoginRequest = new LoginRequest();
    }
    
    public LoginViewModel()
    {
        _memoryCache = null!;
        _employeesWebApi = null!;
        _identityWebApi = null!;
        
        LoginRequest = new LoginRequest();
    }

    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        var authResponse = await _identityWebApi.LoginAsync(LoginRequest);
        if (!authResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Incorrect Credentials", 
                "Please your data and try again!",
                SukiToastType.Error,
                TimeSpan.FromSeconds(3));

            return;
        }

        _memoryCache.Set(authResponse.Content!.Token, CacheKeys.JwtToken);
        
        var loggedInEmployeeResponse = await _employeesWebApi.GetByUsernameAsync(LoginRequest.UserName);
        if (!loggedInEmployeeResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Unexpected Error", 
                "We have encountared unexpected error. Please wait until it resolves and try again.",
                SukiToastType.Error,
                TimeSpan.FromSeconds(3));

            return;
        }

        _memoryCache.Set(loggedInEmployeeResponse.Content!, CacheKeys.Employee);
        MoveToMainDashBoard();
    }

    private static void MoveToMainDashBoard()
    {
        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new MainWindow();
        mainWindow.Show();

        desktop.MainWindow!.Close();
        desktop.MainWindow = mainWindow;
    }
}