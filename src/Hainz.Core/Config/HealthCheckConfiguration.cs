namespace Hainz.Core.Config;

public class HealthCheckConfiguration
{
    public bool IsEnabled { get; init; }
    public int InitialTimeout { get; init; }
    public int Timeout { get; init; }
    public int Interval { get; init; }
    public int UnhealthyInterval { get; init; }
}