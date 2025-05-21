using Application.Common.Interfaces.Services;

namespace Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    
    public DateTime UtcNow => DateTime.UtcNow;
}