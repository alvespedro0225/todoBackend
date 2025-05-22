namespace Application.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTime Offset { get; }
}