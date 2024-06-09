using System.Collections.Generic;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Trips;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class TripsViewModel : ViewModelBase
{
    [ObservableProperty]
    private CreateTripRequest _createTripRequest;

    [ObservableProperty]
    private IEnumerable<Trip> _trips;
    
    private readonly Employee _employee;

    public TripsViewModel(IMemoryCache memoryCache)
    {
        CreateTripRequest = new CreateTripRequest();
        _employee = memoryCache.Get<Employee>(CacheKeys.Employee)!;
        Trips = _employee.Trips ?? [];
    }
    
    public TripsViewModel()
    {
        CreateTripRequest = new CreateTripRequest();

        _employee = null!;
        Trips = AutoFaker.Generate<Trip>(10);
    }

    [RelayCommand]
    private void OpenCreateNewTripDialog()
    {
        var createNewTripUserControl = App.Services.GetRequiredService<CreateNewTripUserControl>();

        SukiHost.ShowDialog(createNewTripUserControl, allowBackgroundClose:true);
    }
}