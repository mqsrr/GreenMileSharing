using FluentValidation;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Trips;

namespace GreenMileSharing.TripApi.Application.Validators.Trips;

internal sealed class CreateTripRequestValidator : AbstractValidator<CreateTripRequest>
{
    public CreateTripRequestValidator()
    {
        RuleFor(request => request.StartMileage)
            .NotNull();

        RuleFor(request => request.EndMileage)
            .NotEmpty()
            .GreaterThan(request => request.StartMileage);
    }
}