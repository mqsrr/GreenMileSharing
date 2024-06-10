using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Identity;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.Views;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private LoginRequest _loginRequest;

    private readonly IIdentityWebApi _identityWebApi;
    private readonly IEmployeesWebApi _employeesWebApi;
    
    public LoginViewModel(IIdentityWebApi identityWebApi, IEmployeesWebApi employeesWebApi)
    {
        _identityWebApi = identityWebApi;
        _employeesWebApi = employeesWebApi;

        LoginRequest = new LoginRequest();
    }
    
    public LoginViewModel()
    {
        _employeesWebApi = null!;
        _identityWebApi = null!;
        
        LoginRequest = new LoginRequest();
    }

    private async Task InitializeAsync()
    {
        var storageFile = await TopLevel.GetTopLevel(null)!.StorageProvider.TryGetFileFromPathAsync("~/.storage/credentials");
        if (storageFile is null)
        {
            return;
        }

        await using var stream = await storageFile.OpenReadAsync();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        byte[] encodedToken = memory.ToArray();
        
        string token = Encoding.UTF8.GetString(encodedToken);
    }
    
    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        var authResponse = await _identityWebApi.LoginAsync(LoginRequest, cancellationToken);
        if (!authResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Incorrect Credentials", 
                "Please your data and try again!",
                SukiToastType.Error,
                TimeSpan.FromSeconds(3));

            return;
        }
        
        StaticStorage.Token = authResponse.Content!.Token;
        // await SaveTokenAsync(authResponse.Content!.Token, cancellationToken);
        
        var loggedInEmployeeResponse = await _employeesWebApi.GetByUsernameAsync(LoginRequest.UserName, cancellationToken);
        if (!loggedInEmployeeResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Unexpected Error", 
                "We have encountered unexpected error. Please wait until it resolves and try again.",
                SukiToastType.Error,
                TimeSpan.FromSeconds(3));

            return;
        }

        StaticStorage.Employee = loggedInEmployeeResponse.Content!;
        MoveToMainDashBoard();
    }

    public static void MoveToMainDashBoard()
    {
        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new MainWindow();
        mainWindow.Show();

        desktop.MainWindow!.Close();
        desktop.MainWindow = mainWindow;
    }

    private Task<IStorageFile?> OpenStorageFile(string path)
    {
        return TopLevel.GetTopLevel(null)!.StorageProvider.TryGetFileFromPathAsync(path);
    } 
    
    private async Task SaveTokenAsync(string token, CancellationToken cancellationToken)
    {
        var storageFile = await OpenStorageFile("~/.storage/credentials");
        if (storageFile is null)
        {
            return;
        }
        
        await using var stream = await storageFile.OpenWriteAsync();
        byte[] tokenData = Encoding.UTF8.GetBytes(token);

        await stream.WriteAsync(tokenData, cancellationToken);
        stream.Close();
    }
}