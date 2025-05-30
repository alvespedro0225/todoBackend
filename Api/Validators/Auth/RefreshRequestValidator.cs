using Api.Models.Request.Auth;

using Api.Models.Request;

using FluentValidation;

namespace Api.Validators.Auth;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty();
        
        RuleFor(request => request.Id)
            .NotEmpty();
    }
}