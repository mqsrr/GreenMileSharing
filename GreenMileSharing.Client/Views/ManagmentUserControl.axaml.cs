using Avalonia.Controls;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GreenMileSharing.Client.Views;

public partial class ManagementUserControl : UserControl
{
    private readonly ManagementViewModel _viewModel;
    
    public ManagementUserControl()
    {
        InitializeComponent();
        _viewModel = App.Services.GetRequiredService<ManagementViewModel>();
        DataContext = _viewModel;
    }

    protected override async void OnInitialized()
    {
        await _viewModel.InitializeAsync();
    }
}