using FluentValidation;
using GreenMileSharing.TripApi.Application.Contracts.Requests.Cars;

namespace GreenMileSharing.TripApi.Application.Validators.Cars;

internal sealed class CreateCarRequestValidator : AbstractValidator<CreateCarRequest>
{
    public CreateCarRequestValidator()
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
            .NotNull();
        
        RuleFor(car => car.EndOfLifeMileage)
            .NotEmpty()
            .GreaterThan(car => car.CurrentMileage);
        
        RuleFor(car => car.MaintenanceInterval)
            .NotEmpty()
            .GreaterThan(car => car.CurrentMileage)
            .LessThan(car => car.EndOfLifeMileage);
    }
}