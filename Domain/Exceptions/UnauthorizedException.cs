namespace Domain.Exceptions;

public sealed class UnauthorizedException(string error, string message) : Exception(message), IHttpException
{
    public string? Error { get; } = error;
    public int StatusCode => 401;
    public string Type => "Unauthorized";
}