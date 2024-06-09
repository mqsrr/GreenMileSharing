using System;

namespace GreenMileSharing.Client.Contracts.Trips;

internal sealed class CreateTripRequest
{
    public Guid EmployeeId { get; init; }
    
    public Guid CarId { get; init; }

    public string StartMileage { get; init; } = null!;

    public string EndMileage { get; init; } = null!;

}