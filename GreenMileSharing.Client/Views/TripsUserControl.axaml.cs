using Avalonia.Controls;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Client.Views;

public partial class TripsUserControl : UserControl
{
    public TripsUserControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<TripsViewModel>();
    }
}