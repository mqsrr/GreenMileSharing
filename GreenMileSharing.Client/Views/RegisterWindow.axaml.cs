using Avalonia.Input;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace GreenMileSharing.Client.Views;

public partial class RegisterWindow : SukiWindow
{
    public RegisterWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<RegisterViewModel>();
    }

    private void CloseWindow(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }
}