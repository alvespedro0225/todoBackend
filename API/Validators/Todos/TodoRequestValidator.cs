using API.Models.Request.Todos;
using FluentValidation;

namespace API.Validators.Todos;

internal class TodoRequestValidator : AbstractValidator<TodoRequest>
{
    internal TodoRequestValidator()
    {
        RuleFor(todoRequest => todoRequest.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(40);

        RuleFor(todoRequest => todoRequest.Description)
            .NotNull()
            .MaximumLength(200);

        RuleFor(todoRequest => todoRequest.Status)
            .NotEmpty()
            .IsInEnum();
    }
}