namespace Domain.Exceptions;

public interface IHttpException
{
    public string? Error { get; }
    public int StatusCode { get; }
    public string Type { get; }
    public string Message { get; }

}