using Api.Models.Request.Auth;

using Api.Models.Request;

using FluentValidation;

namespace Api.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(50);

        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(40);
    }
}