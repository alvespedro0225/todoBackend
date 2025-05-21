using API.Models.Request;
using FluentValidation;

namespace API.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotNull()
            .MaximumLength(50);

        RuleFor(request => request.Password)
            .NotNull()
            .MinimumLength(8)
            .MaximumLength(40);
    }
}