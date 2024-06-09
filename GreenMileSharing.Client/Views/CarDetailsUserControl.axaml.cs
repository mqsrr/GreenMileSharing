using Avalonia.Controls;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.ViewModels;

namespace GreenMileSharing.Client.Views;

internal partial class CarDetailsUserControl : UserControl
{
    public CarDetailsUserControl()
    {
        InitializeComponent();
    }
    
    public CarDetailsUserControl(Car car)
    {
        InitializeComponent();
        DataContext = new CarDetailsViewModel(car);
    }
}