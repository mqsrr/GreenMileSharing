using GreenMileSharing.Client.Models;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands;

public sealed class CreateCarCommand : ICommand
{
    public required Car Car { get; init; }
}