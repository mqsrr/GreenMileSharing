using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private Employee _employee = null!;
    
    [ObservableProperty]
    private ObservableCollection<Car> _cars = null!;
    
    public DashboardViewModel()
    {
        Employee = StaticStorage.Employee;
        Cars = new ObservableCollection<Car>(StaticStorage.Cars);
    }

    [RelayCommand]
    private static void OpenCreateCarUserControl()
    {
        var createCarUserControl = App.Services.GetRequiredService<CreateCarUserControl>();
        SukiHost.ShowDialog(createCarUserControl, allowBackgroundClose:true);
    }
    
    [RelayCommand]
    private void ReloadCars()
    {
        Cars = new ObservableCollection<Car>(StaticStorage.Cars);
    }
    
    
}

