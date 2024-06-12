using System.Collections.Generic;
using System.Linq;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Trips;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class TripsViewModel : ViewModelBase
{
    [ObservableProperty]
    private CreateTripRequest _createTripRequest;

    [ObservableProperty]
    private List<Trip> _trips;

    public TripsViewModel()
    {
        CreateTripRequest = new CreateTripRequest();
        Trips = StaticStorage.Employee.Trips?
            .Select(trip =>
            {
                var tripCar = StaticStorage.Cars.FirstOrDefault(car => car.Id == trip.CarId);
                trip.Car = tripCar;
                
                return trip;
            }).ToList() ?? [];

    }

    [RelayCommand]
    private void OpenCreateNewTripDialog()
    {
        var createNewTripUserControl = App.Services.GetRequiredService<CreateNewTripUserControl>();
        SukiHost.ShowDialog(createNewTripUserControl, allowBackgroundClose: true);
    }
}