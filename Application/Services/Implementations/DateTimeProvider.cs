namespace Application.Services.Implementations;

public sealed class DateTimeProvider : IDateTimeProvider
{
    
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Offset => DateTime.Now;
}