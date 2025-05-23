namespace Domain.Exceptions;

public sealed class ForbiddenException : HttpException
{
    public override string? Error { get; }
    public override int StatusCode => 403;
    public override string Type => "Forbidden Exception";
    
    public ForbiddenException()
    { }
    
    public ForbiddenException(string message) : base(message)
    { }

    public ForbiddenException(string error, string message) : base(message)
    {
        Error = error;
    }
}