using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Trips;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Messages.Commands;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.RefitClients;
using Mediator;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class CreateNewTripViewModel : ViewModelBase
{
    [ObservableProperty]
    private CreateTripRequest _createTripRequest;
    
    [ObservableProperty]
    private Car? _selectedCar;

    [ObservableProperty]
    private ObservableCollection<Car> _cars;

    private readonly ITripsWebApi _tripsWebApi;
    private readonly ISender _sender;    
    
    public CreateNewTripViewModel(ITripsWebApi tripsWebApi, ISender sender)
    {
        _tripsWebApi = tripsWebApi;
        _sender = sender;
        CreateTripRequest = new CreateTripRequest();
        
        SelectedCar = null!;
        Cars = new ObservableCollection<Car>(StaticStorage.Cars);
    }
    
    public CreateNewTripViewModel()
    {
        _sender = null!;
        _tripsWebApi = null!;
        CreateTripRequest = new CreateTripRequest();
        
        SelectedCar = null!;
        Cars = new ObservableCollection<Car>(AutoFaker.Generate<Car>(10));
    }

    [RelayCommand]
    private async Task CreateTripAsync(CancellationToken cancellationToken)
    {
        if (SelectedCar is null)
        {
            return;
        }
        
        CreateTripRequest.CarId = SelectedCar.Id;
        CreateTripRequest.EmployeeId = StaticStorage.Employee.Id;
        CreateTripRequest.StartMileage = SelectedCar.CurrentMileage;
        
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
        
        var createdTrip = response.Content!;
        SelectedCar.CurrentMileage = Convert.ToInt32(CreateTripRequest.EndMileage);
            
        createdTrip.Car = SelectedCar;
        await _sender.Send(new CreateTripCommand
        {
            Trip = createdTrip
        }, cancellationToken).ConfigureAwait(false);
        
        SukiHost.CloseDialog();
    }
}