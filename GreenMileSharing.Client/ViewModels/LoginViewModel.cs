using System.Linq;
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
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private LoginRequest _loginRequest;

    private readonly IIdentityWebApi _identityWebApi;
    private readonly IEmployeesWebApi _employeesWebApi;
    private readonly ICarsWebApi _carsWebApi;
    
    public LoginViewModel(IIdentityWebApi identityWebApi, IEmployeesWebApi employeesWebApi, ICarsWebApi carsWebApi)
    {
        _identityWebApi = identityWebApi;
        _employeesWebApi = employeesWebApi;
        _carsWebApi = carsWebApi;

        LoginRequest = new LoginRequest();
    }
    
    public LoginViewModel()
    {
        _carsWebApi = null!;
        _employeesWebApi = null!;
        _identityWebApi = null!;
        
        LoginRequest = new LoginRequest();
    }
    
    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        var authResponse = await _identityWebApi.LoginAsync(StaticStorage.ApiVersion, LoginRequest, cancellationToken);
        if (!authResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Incorrect Credentials", 
                "Please your data and try again!",
                SukiToastType.Error);

            return;
        }
        
        StaticStorage.Token = authResponse.Content!.Token;
        StaticStorage.IdentityRole = authResponse.Content!.Role;
        
        var loggedInEmployeeResponse = await _employeesWebApi.GetByUsernameAsync(StaticStorage.ApiVersion, LoginRequest.UserName, cancellationToken);
        var carsResponse = await _carsWebApi.GetAllAsync(StaticStorage.ApiVersion, cancellationToken);
        
        if (!loggedInEmployeeResponse.IsSuccessStatusCode || !carsResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Unexpected Error", 
                "We have encountered unexpected error. Please wait until it resolves and try again.",
                SukiToastType.Error);

            return;
        }

        StaticStorage.Employee = loggedInEmployeeResponse.Content!;
        StaticStorage.Cars = carsResponse.Content!.OrderBy(car => car.CarBrand).ToList();
        
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
}