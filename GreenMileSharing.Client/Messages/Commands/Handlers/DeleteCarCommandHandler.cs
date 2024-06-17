using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.ViewModels;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands.Handlers;

internal sealed class DeleteCarCommandHandler(
    DashboardViewModel dashboardViewModel,
    CreateNewTripViewModel createNewTripViewModel,
    ManagementViewModel managementViewModel) : ICommandHandler<DeleteCarCommand>
{
    public ValueTask<Unit> Handle(DeleteCarCommand command, CancellationToken cancellationToken)
    {
        var deletedCar = command.Car;

        dashboardViewModel.Cars.Remove(deletedCar);
        createNewTripViewModel.Cars.Remove(deletedCar);
        managementViewModel.Cars.Remove(deletedCar);
        
        StaticStorage.Cars.Remove(deletedCar);
        return ValueTask.FromResult(Unit.Value);
    }
}