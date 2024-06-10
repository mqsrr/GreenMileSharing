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

public partial class CreateNewTripUserControl : UserControl
{
    
    public CreateNewTripUserControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<CreateNewTripViewModel>();
    }
    
    private void OpenCarDetailsView(object? sender, TappedEventArgs e)
    {
        var tappedGlassCard = sender as GlassCard;
        var tappedCar = tappedGlassCard!.DataContext as Car;
        var carDetails = new CarDetailsUserControl(tappedCar!);

        SukiHost.ShowDialog(carDetails, allowBackgroundClose: true);
    }
}