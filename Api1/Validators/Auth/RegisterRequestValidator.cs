using Api1.Models.Request;

using Api1.Models.Request.Auth;

using FluentValidation;

namespace Api1.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(40);

        RuleFor(user => user.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(40);

        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(40);
    }
}