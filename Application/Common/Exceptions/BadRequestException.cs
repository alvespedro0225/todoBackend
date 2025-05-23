namespace Application.Common.Exceptions;

public sealed class BadRequestException(string error, string message) : Exception(message), IHttpException
{
    public string? Error { get; } = error;
    public int StatusCode => 400;
    public string Type => "Bad Request";
}