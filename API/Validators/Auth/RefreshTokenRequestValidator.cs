using API.Models.Request;
using API.Models.Request.Auth;
using FluentValidation;

namespace API.Validators.Auth;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken).NotEmpty();
        RuleFor(request => request.Id).NotEmpty();
    }
}