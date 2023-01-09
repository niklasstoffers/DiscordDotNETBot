using Hainz.Core.DTOs;

namespace Hainz.Core.Services.Bot;

public sealed class UptimeMonitorService
{
    private readonly object _lock = new();
    private DateTime? _currentDowntimeStart;

    public UptimeStatistic UptimeStatistic { get; init; }

    public UptimeMonitorService()
    {
        UptimeStatistic = new UptimeStatistic();
    }

    public void Connect()
    {
        lock(_lock)
        {
            UptimeStatistic.FirstUptime ??= DateTime.UtcNow;
            UptimeStatistic.IsUp = true;

            if (_currentDowntimeStart != null)
            {
                TimeSpan? downTimeDuration = DateTime.UtcNow - _currentDowntimeStart;

                UptimeStatistic.LastDowntime = _currentDowntimeStart;
                UptimeStatistic.LastDowntimeDuration = downTimeDuration;
                UptimeStatistic.TotalDowntimeDuration += downTimeDuration.Value;

                _currentDowntimeStart = null;
            }
        }
    }

    public void Disconnect()
    {
        lock(_lock)
        {
            UptimeStatistic.IsUp = false;
            _currentDowntimeStart ??= DateTime.UtcNow;
        }
    }
}