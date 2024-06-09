namespace GreenMileSharing.Client.Contracts.Cars;

internal sealed class CreateCarRequest
{
    public required byte[] Image { get; init; }
    
    public required string LicensePlateNumber { get; init; }

    public required string CarBrand { get; init; }

    public required string Model { get; init; }

    public required int EndOfLifeMileage { get; init; }

    public required int MaintenanceInterval { get; init; }

    public required int CurrentMileage { get; init; }
}