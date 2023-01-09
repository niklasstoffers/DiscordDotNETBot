namespace Hainz.Core.DTOs;

public class UptimeStatistic
{
    public bool IsUp { get; set; }
    public DateTime? FirstUptime { get; set; }
    public TimeSpan TotalDowntimeDuration { get; set; }
    public DateTime? LastDowntime { get; set; }
    public TimeSpan? LastDowntimeDuration { get; set; }

    public TimeSpan TotalOperationTime
    {
        get
        {
            if (FirstUptime == null)
                return default;
            
            return (DateTime.UtcNow - FirstUptime).Value;
        }
    }

    public double UptimePercentage
    {
        get
        {
            if (FirstUptime == null)
                return 0;
            
            return (double)(TotalOperationTime - TotalDowntimeDuration).Ticks / TotalOperationTime.Ticks * 100.0;
        }
    }

    public TimeSpan CurrentUptime
    {
        get
        {
            if (FirstUptime == null)
                return default;
                
            return DateTime.UtcNow - (LastDowntime ?? FirstUptime).Value;
        }
    }
}