using GreenMileSharing.Client.Models;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands;

public sealed class CreateTripCommand : ICommand
{
    public required Trip Trip { get; init; }
}