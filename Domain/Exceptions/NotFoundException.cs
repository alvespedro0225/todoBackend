namespace Domain.Exceptions;

public sealed class NotFoundException(string error, string message) : Exception(message), IHttpException
{
    public string? Error { get; } = error;
    public int StatusCode => 404;
    public string Type => "Not Found";
}