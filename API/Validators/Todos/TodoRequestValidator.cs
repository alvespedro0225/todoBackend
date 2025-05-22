using Application.Models.Request.Todos;
using Domain;
using FluentValidation;

namespace API.Validators.Todos;

internal class TodoRequestValidator : AbstractValidator<TodosServiceRequest>
{
    internal TodoRequestValidator()
    {
        RuleFor(todoRequest => todoRequest.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(30);

        RuleFor(todoRequest => todoRequest.Description)
            .NotNull()
            .MaximumLength(200);

        RuleFor(todoRequest => todoRequest.Status)
            .NotEmpty()
            .IsInEnum();
    }
}