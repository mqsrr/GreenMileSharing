using GreenMileSharing.Client.Models;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands;

public sealed class DeleteCarCommand : ICommand
{
    public required Car Car { get; init; }
}