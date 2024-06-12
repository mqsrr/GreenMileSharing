using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.ViewModels;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands.Handlers;

internal sealed class CreateTripCommandHandler(TripsViewModel tripsViewModel) : ICommandHandler<CreateTripCommand>
{
    public ValueTask<Unit> Handle(CreateTripCommand command, CancellationToken cancellationToken)
    {
        var createdTrip = command.Trip;

        tripsViewModel.Trips.Add(createdTrip);
        StaticStorage.Employee.Trips?.Add(createdTrip);

        return Unit.ValueTask;
    }
}