using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.ViewModels;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands.Handlers;

internal sealed class CreateTripCommandHandler(
    TripsViewModel tripsViewModel,
    DashboardViewModel dashboardViewModel,
    CreateNewTripViewModel createNewTripViewModel,
    ManagementViewModel managementViewModel) : ICommandHandler<CreateTripCommand>
{
    public ValueTask<Unit> Handle(CreateTripCommand command, CancellationToken cancellationToken)
    {
        var createdTrip = command.Trip;
        var carToReplace = StaticStorage.Cars.Find(car => car.Id == createdTrip.CarId);
        
        tripsViewModel.Trips.Add(createdTrip);
        createNewTripViewModel.Cars!.Replace(carToReplace, createdTrip.Car);
        
        dashboardViewModel.Cars!.Replace(carToReplace, createdTrip.Car);
        managementViewModel.Cars!.Replace(carToReplace, createdTrip.Car);

        StaticStorage.Cars!.Replace(carToReplace, createdTrip.Car);
        StaticStorage.Employee.Trips?.Add(createdTrip);
        
        return Unit.ValueTask;
    }
}