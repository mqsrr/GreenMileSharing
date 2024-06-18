using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI;
using SukiUI.Controls;

namespace GreenMileSharing.Client.Views;

public partial class MainWindow : SukiWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ClientSize = new Size(1300, 700);
        DataContext = new MainWindowViewModel();
    }

    
    private void OnPrimaryColorChangeClick(object? sender, PointerPressedEventArgs e)
    {
        SukiTheme.GetInstance().SwitchColorTheme();
    }

    private void OnThemeChangeClick(object? sender, PointerPressedEventArgs e)
    {
        SukiTheme.GetInstance().SwitchBaseTheme();
    }
}