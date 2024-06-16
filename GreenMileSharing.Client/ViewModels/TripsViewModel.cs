using System.Collections.ObjectModel;
using System.Linq;
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
    private ObservableCollection<Trip> _trips;

    public TripsViewModel()
    {
        CreateTripRequest = new CreateTripRequest();
        Trips = new ObservableCollection<Trip>(StaticStorage.Employee.Trips?
            .Select(trip =>
            {
                var tripCar = StaticStorage.Cars.FirstOrDefault(car => car.Id == trip.CarId);
                trip.Car = tripCar;
                
                return trip;
            }).OrderBy(trip => trip.CarId) ?? Enumerable.Empty<Trip>());
    }

    [RelayCommand]
    private void OpenCreateNewTripDialog()
    {
        var createNewTripUserControl = App.Services.GetRequiredService<CreateNewTripUserControl>();
        SukiHost.ShowDialog(createNewTripUserControl, allowBackgroundClose: true);
    }
}