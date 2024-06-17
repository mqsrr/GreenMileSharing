using FluentValidation;
using GreenMileSharing.IdentityApi.Application.Contracts.Requests;

namespace GreenMileSharing.IdentityApi.Application.Validation;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(request => request.Username)
            .NotEmpty();
    }
}