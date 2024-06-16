using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.ViewModels;
using Mediator;

namespace GreenMileSharing.Client.Messages.Commands.Handlers;

internal sealed class CreateCarCommandHandler(DashboardViewModel dashboardViewModel, CreateNewTripViewModel createNewTripViewModel) : ICommandHandler<CreateCarCommand>
{
    public ValueTask<Unit> Handle(CreateCarCommand command, CancellationToken cancellationToken)
    {
        var createdCar = command.Car;
        
        dashboardViewModel.Cars.Add(createdCar);
        createNewTripViewModel.Cars.Add(createdCar);
        
        StaticStorage.Cars.Add(createdCar);
        return Unit.ValueTask;
    }
}