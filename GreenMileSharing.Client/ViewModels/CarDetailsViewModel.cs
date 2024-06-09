using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class CarDetailsViewModel : ViewModelBase
{
    [ObservableProperty]
    private Car _car;

    public CarDetailsViewModel(Car car)
    {
        Car = car;
    }
    
    public CarDetailsViewModel()
    {
        Car = AutoFaker.Generate<Car>();
    }
}