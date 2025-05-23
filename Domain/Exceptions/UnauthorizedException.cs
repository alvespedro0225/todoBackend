namespace Domain.Exceptions;

public sealed class UnauthorizedException : HttpException
{
    public override string? Error { get; }
    public override int StatusCode => 401;
    public override string Type => "Unauthorized";
    
    public UnauthorizedException() : base()
    { }
    
    public UnauthorizedException(string message) : base(message)
    { }

    public UnauthorizedException(string error, string message) : base(message)
    {
        Error = error;
    }
}