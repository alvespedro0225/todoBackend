using Api1.Models.Request;

using Api1.Models.Request.Auth;

using FluentValidation;

namespace Api1.Validators.Auth;

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