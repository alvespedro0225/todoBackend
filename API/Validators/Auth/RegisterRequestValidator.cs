using API.Models.Request;
using API.Models.Request.Auth;
using FluentValidation;

namespace API.Validators.Auth;

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