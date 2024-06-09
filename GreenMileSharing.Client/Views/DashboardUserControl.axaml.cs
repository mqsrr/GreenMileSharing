using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace GreenMileSharing.Client.Views;

public partial class DashboardUserControl : UserControl
{
    private readonly DashboardViewModel _dashboardViewModel;

    private readonly IEnumerable<Car> _cars;

    public DashboardUserControl()
    {
        InitializeComponent();
        _dashboardViewModel = App.Services.GetRequiredService<DashboardViewModel>();
        _cars = _dashboardViewModel.Cars;

        DataContext = _dashboardViewModel;
    }

    public void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        string textBoxText = textBox!.Text!;

        _dashboardViewModel.Cars = _cars;
        _dashboardViewModel.Cars = _dashboardViewModel.Cars.Where(car =>
            car.CarBrand.Contains(textBoxText, StringComparison.OrdinalIgnoreCase)
            || car.LicensePlateNumber.Contains(textBoxText, StringComparison.OrdinalIgnoreCase));
    }

    private void OpenCarDetailsView(object? sender, TappedEventArgs e)
    {
        var tappedGlassCard = sender as GlassCard;
        var tappedCar = tappedGlassCard!.DataContext as Car;
        var carDetails = new CarDetailsUserControl(tappedCar!);

        SukiHost.ShowDialog(carDetails, allowBackgroundClose: true);
    }
}