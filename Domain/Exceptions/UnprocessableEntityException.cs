namespace Domain.Exceptions;

public sealed class UnprocessableEntityException : HttpException
{
    public override string? Error { get; }
    public override int StatusCode => 422;
    public override string Type => "Unprocessable Entity";
    
    public UnprocessableEntityException() : base()
    { }

    public UnprocessableEntityException(string message) : base(message)
    { }

    public UnprocessableEntityException(string error, string message) : base(message)
    {
        Error = error;
    }
}