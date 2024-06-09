using FluentValidation;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Cars;

namespace GreenMileSharing.TripApi.Application.Validators.Cars;

internal sealed class UpdateCarRequestValidator : AbstractValidator<UpdateCarRequest>
{
    public UpdateCarRequestValidator()
    {
        RuleFor(car => car.CarBrand)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(car => car.Model)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(car => car.LicensePlateNumber)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(car => car.CurrentMileage)
            .NotEmpty();
        
        RuleFor(car => car.EndOfLifeMileage)
            .NotEmpty();
        
        RuleFor(car => car.MaintenanceInterval)
            .NotEmpty();

    }
}