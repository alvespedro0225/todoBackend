namespace Application.Services.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Offset => DateTime.Now;
}