using Clinio.Application.Interfaces;

namespace Clinio.Infrastructure.Services;

public class ClinicTimeZone : IClinicClock
{
    private static readonly TimeZoneInfo Zone = ResolveZone();

    private static TimeZoneInfo ResolveZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows() ? "Egypt Standard Time" : "Africa/Cairo");
        }
        catch (TimeZoneNotFoundException)
        {
            // fallback in case the OS timezone database doesn't have it
            return TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows() ? "Africa/Cairo" : "Egypt Standard Time");
        }
    }

    public DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Zone);
    public DateOnly Today => DateOnly.FromDateTime(Now);
}