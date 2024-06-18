using CommunityToolkit.Mvvm.ComponentModel;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static bool IsManager => StaticStorage.IdentityRole == "Manager";
}