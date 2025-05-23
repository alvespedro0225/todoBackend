namespace Domain.Exceptions;

public sealed class NotFoundException : HttpException
{
    public override string? Error { get; }
    public override int StatusCode => 404;
    public override string Type => "Not Found";

    public NotFoundException() : base()
    { }

    public NotFoundException(string message) : base(message)
    { }

    public NotFoundException(string error, string message) : base(message)
    {
        Error = error;
    }
}