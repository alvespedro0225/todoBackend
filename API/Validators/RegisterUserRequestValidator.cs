using API.Models.Request;
using FluentValidation;

namespace API.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(user => user.Name)
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(40);

        RuleFor(user => user.Password)
            .NotNull()
            .MinimumLength(8)
            .MaximumLength(40);

        RuleFor(user => user.Email)
            .NotNull()
            .MaximumLength(40);
    }
}