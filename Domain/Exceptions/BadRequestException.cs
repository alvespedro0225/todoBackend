namespace Domain.Exceptions;

public sealed class BadRequestException : HttpException
{
    public override string? Error { get; }
    public override int StatusCode => 400;
    public override string Type => "Bad Request";

    public BadRequestException() : base()
    { }
    
    public BadRequestException(string message) : base(message)
    { }

    public BadRequestException(string error, string message) : base(message)
    {
        Error = error;
    }
}