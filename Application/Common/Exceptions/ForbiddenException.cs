namespace Application.Common.Exceptions;

public sealed class ForbiddenException(string error, string message) : Exception(message), IHttpException
{
    public string? Error { get; } = error;
    public int StatusCode => 403;
    public string Type => "Forbidden Exception";
}