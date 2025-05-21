using API.Models.Request;
using FluentValidation;

namespace API.Validators;

internal class CreateTodoRequestValidators : AbstractValidator<CreateTodoRequest>
{
    internal CreateTodoRequestValidators(CreateTodoRequest request)
    {
        RuleFor(todoRequest => todoRequest.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(30);

        RuleFor(todoRequest => todoRequest.Description)
            .NotNull()
            .MaximumLength(200);

        RuleFor(todoRequest => todoRequest.Status)
            .NotEmpty();
    }
}