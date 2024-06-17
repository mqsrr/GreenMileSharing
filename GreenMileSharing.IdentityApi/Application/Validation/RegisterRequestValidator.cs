using FluentValidation;
using RegisterRequest = GreenMileSharing.IdentityApi.Application.Contracts.Requests.RegisterRequest;

namespace GreenMileSharing.IdentityApi.Application.Validation;

internal sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(6);
        
        RuleFor(request => request.UserName)
            .NotEmpty();
        
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress();
    }
}