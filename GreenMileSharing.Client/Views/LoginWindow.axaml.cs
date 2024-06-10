using System;
using Avalonia.Controls;
using Avalonia.Input;
using GreenMileSharing.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;

namespace GreenMileSharing.Client.Views;

internal partial class LoginWindow : SukiWindow
{
    private readonly RegisterWindow _registerView;
    
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<LoginViewModel>();
        _registerView = App.Services.GetRequiredService<RegisterWindow>();
        
        _registerView.Closing += OnClosingRegisterView;
        Closing += (_, _) => _registerView.Closing -= OnClosingRegisterView;
    }
    
    protected override void OnClosed(EventArgs e)
    {
        _registerView.Close();
    }

    private void MoveToRegisterWindow(object? sender, PointerPressedEventArgs e)
    {
        Hide();
        _registerView.Show();
    }

    private void OnClosingRegisterView(object? o, WindowClosingEventArgs args)
    {
        ((SukiWindow)o!).Hide();
        args.Cancel = true;
        Show();
    }
}