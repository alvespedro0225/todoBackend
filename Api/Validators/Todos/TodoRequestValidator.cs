using Api.Models.Request.Todos;

using FluentValidation;

namespace Api.Validators.Todos;

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
            .NotNull()
            .IsInEnum();
    }
}