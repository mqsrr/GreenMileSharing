using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private Employee _employee = null!;

    [ObservableProperty]
    private IEnumerable<Car> _cars = null!;

    private readonly ICarsWebApi _carsWebApi;

    public DashboardViewModel(ICarsWebApi carsWebApi)
    {
        _carsWebApi = carsWebApi;

        Employee = StaticStorage.Employee;
    }

    public DashboardViewModel()
    {
        _carsWebApi = null!;
        
        Employee = AutoFaker.Generate<Employee>();
        Cars = AutoFaker.Generate<Car>(10);
    }
    
    internal async Task InitializeAsync()
    {
        var carsResponse = await _carsWebApi.GetAllAsync(CancellationToken.None);
        if (!carsResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure", "Cars information couldn't be fetched", SukiToastType.Error);
            return;
        }

        Cars = carsResponse.Content!;
    }

    [RelayCommand]
    private void OpenCreateCarUserControl()
    {
        var createCarUserControl = App.Services.GetRequiredService<CreateCarUserControl>();
        SukiHost.ShowDialog(createCarUserControl, allowBackgroundClose:true);
    }
}

