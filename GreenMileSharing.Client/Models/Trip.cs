using System;
using Avalonia.Markup.Xaml.Templates;

namespace GreenMileSharing.Client.Models;

internal sealed class Trip
{
    public required Guid Id { get; init; }

    public required Guid EmployeeId { get; init; }
    
    public required Guid CarId { get; init; }
    
    public required int StartMileage { get; init; }

    public required int EndMileage { get; init; }

    public Car? Car { get; set; }
}