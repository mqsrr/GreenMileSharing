using GreenMileSharing.Domain;

namespace GreenMileSharing.TripApi.Application.Contracts.Requests.Cars;

public sealed class UpdateCarRequest
{
    public Guid Id { get; set; }
    
    public required IFormFile Image { get; init; }
    
    public required string LicensePlateNumber { get; init; }

    public required string CarBrand { get; init; }

    public required string Model { get; init; }

    public required int EndOfLifeMileage { get; init; }

    public required int MaintenanceInterval { get; init; }

    public required int CurrentMileage { get; init; }

    public required IEnumerable<Trip> Trips { get; init; }  
}