namespace GreenMileSharing.Domain;

public sealed class Employee
{
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public IEnumerable<Trip>? Trips { get; set; }
}