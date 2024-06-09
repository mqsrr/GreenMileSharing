namespace GreenMileSharing.TripApi.Application.Contracts.Requests.Cars;

public sealed class CreateCarRequest
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public required IFormFile Image { get; init; }
    
    public required string LicensePlateNumber { get; init; }

    public required string CarBrand { get; init; }

    public required string Model { get; init; }

    public required int EndOfLifeMileage { get; init; }

    public required int MaintenanceInterval { get; init; }

    public required int CurrentMileage { get; init; }
}