﻿namespace GreenMileSharing.Domain;

public sealed class Car
{
    public required Guid Id { get; init; }

    public required byte[] Image { get; init; }
    
    public required string LicensePlateNumber { get; init; }

    public required string CarBrand { get; init; }

    public required string Model { get; init; }

    public required int EndOfLifeMileage { get; set; }

    public required int MaintenanceInterval { get; init; }

    public required int CurrentMileage { get; set; }

    public IEnumerable<Trip>? Trips { get; set; }  
}