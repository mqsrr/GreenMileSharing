using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Identity;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.RefitClients;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal partial class RegisterViewModel : ViewModelBase
{
    [ObservableProperty]
    private RegisterRequest _registerRequest;

    private readonly IIdentityWebApi _identityWebApi;
    private readonly IEmployeesWebApi _employeesWebApi;

    public RegisterViewModel()
    {
        _identityWebApi = null!;
        _employeesWebApi = null!;
        
        _registerRequest = new RegisterRequest();
    }
    
    public RegisterViewModel(IIdentityWebApi identityWebApi, IEmployeesWebApi employeesWebApi)
    {
        _identityWebApi = identityWebApi;
        _employeesWebApi = employeesWebApi;
        
        _registerRequest = new RegisterRequest();
    }

    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken)
    {
        var authResponse = await _identityWebApi.RegisterAsync(RegisterRequest, cancellationToken);
        if (!authResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Register Failure!",
                "We couldn't register an account with given credentials!",
                SukiToastType.Error);

            return;
        }

        StaticStorage.Token = authResponse.Content!.Token;
        var employeeResponse = await _employeesWebApi.GetByUsernameAsync(RegisterRequest.UserName, cancellationToken);
        if (!employeeResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Register Failure!",
                "User has been created, but credentials couldn't be fetched! Try to log in.",
                SukiToastType.Error);

            return;
        }

        StaticStorage.Employee = employeeResponse.Content!;
        LoginViewModel.MoveToMainDashBoard();
    }
}