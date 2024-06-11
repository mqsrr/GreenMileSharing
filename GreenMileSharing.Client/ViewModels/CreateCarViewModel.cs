using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GreenMileSharing.Client.Contracts.Cars;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.Views;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class CreateCarViewModel : ViewModelBase
{
    [ObservableProperty]
    private CreateCarRequest _request;

    [ObservableProperty]
    private byte[] _carImage;

    private readonly ICarsWebApi _carsWebApi;

    public CreateCarViewModel(ICarsWebApi carsWebApi)
    {
        _carsWebApi = carsWebApi;
        _carImage = null!;
        Request = new CreateCarRequest();
    }

    public CreateCarViewModel()
    {
        _carsWebApi = null!;
        _carImage = null!;
        Request = new CreateCarRequest();
    }

    [RelayCommand]
    private async Task OpenFileDialogAsync(CancellationToken cancellationToken)
    {
        var createCarUserControl = App.Services.GetRequiredService<CreateCarUserControl>();
        var storageProvider = TopLevel.GetTopLevel(createCarUserControl)!.StorageProvider;
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choose car's photo",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("png"),
                new FilePickerFileType("jpg"),
            ]
        });

        if (!files.Any())
        {
            Console.WriteLine("Null value!");
            return;
        }

        using var memoryStream = new MemoryStream();
        await using var photoStream = await files[0].OpenReadAsync();
        
        await photoStream.CopyToAsync(memoryStream, cancellationToken);
        
        CarImage = memoryStream.ToArray();
        Request.Image = CarImage;
    }

    [RelayCommand]
    private async Task CreateCarAsync(CancellationToken cancellationToken)
    {
        var response = await _carsWebApi.CreateAsync(Request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure", "We couldn't create car. Please Try Again", SukiToastType.Error);
            return;
        }
        
        await SukiHost.ShowToast("Success", "Car has been created!", SukiToastType.Success);
        SukiHost.CloseDialog();
    }
}