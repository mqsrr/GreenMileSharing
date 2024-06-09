using System.Collections.Generic;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using GreenMileSharing.Client.Contracts.Trips;
using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class CreateNewTripViewModel : ViewModelBase
{
    [ObservableProperty]
    private CreateTripRequest _createTripRequest;

    [ObservableProperty]
    private Car _selectedCar;

    [ObservableProperty]
    private IEnumerable<Car> _cars;
    
    public CreateNewTripViewModel()
    {
        _createTripRequest = new CreateTripRequest();
        _selectedCar = null!;
        
        _cars = AutoFaker.Generate<Car>(10);
    }
}