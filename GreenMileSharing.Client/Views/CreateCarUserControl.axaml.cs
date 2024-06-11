using Avalonia.Controls;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Client.Views;

public partial class CreateCarUserControl : UserControl
{
    public CreateCarUserControl()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<CreateCarViewModel>();
    }
}