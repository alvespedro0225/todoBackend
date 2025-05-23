namespace Domain.Exceptions;

public abstract class HttpException : Exception
{
    public abstract string? Error { get; }
    public abstract int StatusCode { get; }
    public abstract string Type { get; }

    public HttpException()
    { }
    
    public HttpException(string message) : base(message)
    { }

    public HttpException(string error, string message) : base(message)
    { }
}