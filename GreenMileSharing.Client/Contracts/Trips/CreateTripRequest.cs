using System;

namespace GreenMileSharing.Client.Contracts.Trips;

internal sealed class CreateTripRequest
{
    public Guid EmployeeId { get; set; }
    
    public Guid CarId { get; set; }

    public int StartMileage { get; set; }

    public string EndMileage { get; init; } = null!;

}