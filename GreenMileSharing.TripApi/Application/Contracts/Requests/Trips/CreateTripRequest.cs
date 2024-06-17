namespace GreenMileSharing.TripApi.Application.Contracts.Requests.Trips;

public sealed class CreateTripRequest
{
    public Guid Id { get; } = Guid.NewGuid();

    public required Guid EmployeeId { get; init; }
    
    public required Guid CarId { get; init; }

    public required int StartMileage { get; init; }

    public required int EndMileage { get; init; }
}