using GreenMileSharing.Client.Models;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands;

internal sealed class DeleteEmployeeCommand : ICommand
{
    public required Employee Employee { get; init; }
}