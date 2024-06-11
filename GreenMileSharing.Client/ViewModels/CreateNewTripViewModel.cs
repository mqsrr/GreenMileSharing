using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Trips;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.RefitClients;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class CreateNewTripViewModel : ViewModelBase
{
    [ObservableProperty]
    private CreateTripRequest _createTripRequest;

    [ObservableProperty]
    private Car _selectedCar;

    [ObservableProperty]
    private IEnumerable<Car> _cars;

    private readonly ITripsWebApi _tripsWebApi;
    
    public CreateNewTripViewModel(ITripsWebApi tripsWebApi)
    {
        _tripsWebApi = tripsWebApi;
        CreateTripRequest = new CreateTripRequest();
        
        SelectedCar = null!;
        Cars = Enumerable.Empty<Car>();
    }
    
    public CreateNewTripViewModel()
    {
        _tripsWebApi = null!;
        CreateTripRequest = new CreateTripRequest();
        
        SelectedCar = null!;
        Cars = AutoFaker.Generate<Car>(10);
    }

    [RelayCommand]
    private async Task CreateTripAsync(CancellationToken cancellationToken)
    {
        var response = await _tripsWebApi.CreateAsync(CreateTripRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast(
                "Failure",
                "Trip couldn't be created! Please try again!",
                SukiToastType.Error);
            return;
        }

        await SukiHost.ShowToast("Success", "Trip has been registered!", SukiToastType.Success);
        SukiHost.CloseDialog();
    }
}