using FluentValidation;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Trips;

namespace GreenMileSharing.TripApi.Application.Validators.Trips;

internal sealed class CreateTripRequestValidator : AbstractValidator<CreateTripRequests>
{
    public CreateTripRequestValidator()
    {
        /*RuleFor(trip => trip.Car)
            .NotNull();

        RuleFor(trip => trip.Employee)
            .NotEmpty();

        RuleFor(trip => trip.StartMileage)
            .NotEmpty();
        
        RuleFor(trip => trip.EndMileage)
            .NotEmpty();*/
    }    
}