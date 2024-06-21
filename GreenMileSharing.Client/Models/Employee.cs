using System;
using System.Collections.Generic;

namespace GreenMileSharing.Client.Models;

internal sealed class Employee
{
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public ICollection<Trip>? Trips { get; set; }
}