namespace Application.Common.Exceptions;

public sealed class UnprocessableEntityException(string error, string message) : Exception(message), IHttpException
{
    public string? Error { get; } = error;
    public int StatusCode => 422;
    public string Type => "Unprocessable Entity";
    
}