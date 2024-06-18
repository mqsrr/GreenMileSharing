using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Messages.Commands;
using GreenMileSharing.Client.Models;
using GreenMileSharing.Client.RefitClients;
using Mediator;
using SukiUI.Controls;
using SukiUI.Enums;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class ManagementViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Employee> _employees;
    
    [ObservableProperty]
    private Employee _selectedEmployee;
    
    [ObservableProperty]
    private ObservableCollection<Car> _cars;
    
    [ObservableProperty]
    private Car _selectedCar;

    private readonly IEmployeesWebApi _employeesWebApi;
    private readonly ICarsWebApi _carsWebApi;
    private readonly ISender _sender;

    public ManagementViewModel(IEmployeesWebApi employeesWebApi, ICarsWebApi carsWebApi, ISender sender)
    {
        Employees = new ObservableCollection<Employee>();
        Cars = new ObservableCollection<Car>(StaticStorage.Cars);
        
        _employeesWebApi = employeesWebApi;
        _carsWebApi = carsWebApi;
        _sender = sender;

        SelectedCar = null!;
        SelectedEmployee = null!;
    }

    public ManagementViewModel()
    {
        Employees = new ObservableCollection<Employee>(AutoFaker.Generate<Employee>(10));
        Cars = new ObservableCollection<Car>(AutoFaker.Generate<Car>(10));

        _employeesWebApi = null!;
        _carsWebApi = null!;
        _sender = null!;
        
        SelectedCar = null!;
        SelectedEmployee = null!;
    }
    
    internal async Task InitializeAsync()
    {
        var employeesResponse = await _employeesWebApi.GetAllAsync(StaticStorage.ApiVersion, CancellationToken.None);
        if (!employeesResponse.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure", "Employees information couldn't be fetched", SukiToastType.Error);
            return;
        }

        var employees = employeesResponse.Content!.ToList();
        int employeeToExclude = employees.FindIndex(employee => employee.Id == StaticStorage.Employee.Id);
        
        employees.RemoveAt(employeeToExclude);
        Employees.AddRange(employees);
    }

    [RelayCommand]
    private async Task DeleteEmployeeAsync(CancellationToken cancellationToken)
    {
        var response = await _employeesWebApi.DeleteByIdAsync(StaticStorage.ApiVersion, SelectedEmployee.Id, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure", "Employee couldn't be deleted!", SukiToastType.Error);
            return;
        }

        Employees.Remove(SelectedEmployee);
        SelectedEmployee = null!;
    }
    
    [RelayCommand]
    private async Task DeleteCarAsync(CancellationToken cancellationToken)
    {
        var response = await _carsWebApi.DeleteByIdAsync(StaticStorage.ApiVersion, SelectedCar.Id, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await SukiHost.ShowToast("Failure", "Car couldn't be deleted!", SukiToastType.Error);
            return;
        }

        await _sender.Send(new DeleteCarCommand
        {
            Car = SelectedCar
        }, cancellationToken);
    }
}