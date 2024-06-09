using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Identity;

namespace GreenMileSharing.Client.ViewModels;

internal partial class RegisterViewModel : ViewModelBase
{
    [ObservableProperty]
    private RegisterRequest _registerRequest;

    public RegisterViewModel()
    {
        _registerRequest = new RegisterRequest();
    }

    [RelayCommand]
    public async Task RegisterAsync(CancellationToken cancellationToken)
    {
        
    }
}