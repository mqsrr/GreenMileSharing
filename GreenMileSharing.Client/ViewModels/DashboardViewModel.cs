using System;
using System.Collections.Generic;
using AutoBogus;
using CommunityToolkit.Mvvm.ComponentModel;
using GreenMileSharing.Client.Models;

namespace GreenMileSharing.Client.ViewModels;

internal sealed partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private Employee _employee;

    [ObservableProperty]
    private IEnumerable<Car> _cars;

    public DashboardViewModel()
    {
        Employee = new Employee
        {
            Id = Guid.NewGuid(),
            Username = "null",
            Trips = null
        };
        
        Cars = AutoFaker.Generate<Car>(10);
    }
}

