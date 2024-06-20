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
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Messages.Commands;
using GreenMileSharing.Client.RefitClients;
using GreenMileSharing.Client.Views;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class CreateCarViewModel : ViewModelBase
{
    [ObservableProperty] 
    private CreateCarRequest _request = new CreateCarRequest();
    
    [ObservableProperty] 
    private byte[] _carImage;

    private readonly ICarsWebApi _carsWebApi;
    private readonly ISender _sender;

    public CreateCarViewModel(ICarsWebApi carsWebApi, ISender sender)
    {
        _carsWebApi = carsWebApi;
        _sender = sender;
        _carImage = null!;
    }

    public CreateCarViewModel()
    {
        _carsWebApi = null!;
        _sender = null!;
        _carImage = null!;
    }

    [RelayCommand]
    private async Task OpenFileDialogAsync(CancellationToken cancellationToken)
    {
        var createCarUserControl = App.Services.GetRequiredService<CreateCarUserControl>();
        var storageProvider = TopLevel.GetTopLevel(createCarUserControl)!.StorageProvider;
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choose car's photo",
            AllowMultiple = false
        });

        if (!files.Any())
        {
            return;
        }

        var memoryStream = new MemoryStream();

        await using var photoStream = await files[0].OpenReadAsync();
        await photoStream.CopyToAsync(memoryStream, cancellationToken);
        
        Request.Image = new StreamPart(memoryStream, files[0].Name);
        CarImage = memoryStream.ToArray();
        
        memoryStream.Position = 0;
    }

    [RelayCommand]
    private async Task CreateCarAsync(CancellationToken cancellationToken)
    {
        CarImage = null!;
        var response = await _carsWebApi.CreateAsync(
            StaticStorage.ApiVersion,
            Request.Image,
            Request.LicensePlateNumber,
            Request.CarBrand,
            Request.Model,
            Convert.ToInt32(Request.EndOfLifeMileage),
            Convert.ToInt32(Request.MaintenanceInterval),
            Convert.ToInt32(Request.CurrentMileage)
        );
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.Error.Content);
            await SukiHost.ShowToast("Failure", "We couldn't create car. Please Try Again", SukiToastType.Error);
            return;
        }

        await SukiHost.ShowToast("Success", "Car has been created!", SukiToastType.Success);
        await _sender.Send(new CreateCarCommand
        {
            Car = response.Content!
        }, cancellationToken);
        
        SukiHost.CloseDialog();
    }
}