using API.Models.Request;
using API.Models.Request.Auth;
using FluentValidation;

namespace API.Validators.Auth;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(request => request.RefreshToken).NotEmpty();
        RuleFor(request => request.Id).NotEmpty();
    }
}