using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.ViewModels;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands.Handlers;

internal sealed class DeleteCarCommandHandler(
    DashboardViewModel dashboardViewModel,
    CreateNewTripViewModel createNewTripViewModel,
    ManagementViewModel managementViewModel,
    TripsViewModel tripsViewModel) : ICommandHandler<DeleteCarCommand>
{
    public ValueTask<Unit> Handle(DeleteCarCommand command, CancellationToken cancellationToken)
    {
        var deletedCar = command.Car;

        dashboardViewModel.Cars.Remove(deletedCar);
        createNewTripViewModel.Cars.Remove(deletedCar);
        managementViewModel.Cars.Remove(deletedCar);
        
        var tripsToDelete = tripsViewModel.Trips.Where(trip => trip.CarId == deletedCar.Id);
        tripsViewModel.Trips.RemoveMany(tripsToDelete);
        
        StaticStorage.Cars.Remove(deletedCar);
        return ValueTask.FromResult(Unit.Value);
    }
}