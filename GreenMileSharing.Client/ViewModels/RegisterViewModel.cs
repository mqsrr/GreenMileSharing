using System.Linq;
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
    private readonly ICarsWebApi _carsWebApi;

    public RegisterViewModel()
    {
        _identityWebApi = null!;
        _employeesWebApi = null!;
        _carsWebApi = null!;
        
        _registerRequest = new RegisterRequest();
    }
    
    public RegisterViewModel(IIdentityWebApi identityWebApi, IEmployeesWebApi employeesWebApi, ICarsWebApi carsWebApi)
    {
        _identityWebApi = identityWebApi;
        _employeesWebApi = employeesWebApi;
        _carsWebApi = carsWebApi;
        
        _registerRequest = new RegisterRequest();
    }

    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken)
    {
        var authResponse = await _identityWebApi.RegisterAsync(StaticStorage.ApiVersion, RegisterRequest, cancellationToken);
        if (!authResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Register Failure!",
                "We couldn't register an account with given credentials!",
                SukiToastType.Error);

            return;
        }

        StaticStorage.Token = authResponse.Content!.Token;
        StaticStorage.IdentityRole = authResponse.Content!.Role;

        var employeeResponse = await _employeesWebApi.GetByUsernameAsync(StaticStorage.ApiVersion, RegisterRequest.UserName, cancellationToken);
        var carsResponse = await _carsWebApi.GetAllAsync(StaticStorage.ApiVersion, cancellationToken);
        
        if (!employeeResponse.IsSuccessStatusCode || !carsResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Register Failure!",
                "User has been created, but credentials couldn't be fetched! Try to log in.",
                SukiToastType.Error);

            return;
        }

        StaticStorage.Employee = employeeResponse.Content!;
        StaticStorage.Cars = carsResponse.Content!.OrderBy(car => car.CarBrand).ToList();
        
        LoginViewModel.MoveToMainDashBoard();
    }
}