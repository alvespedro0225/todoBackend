using FluentValidation.Results;

namespace API.Models;

public sealed record ErrorModel
{
    public string Message { get; set; } = null!;
    public List<ValidationFailure>? ValidationErrors { get; set; }
}