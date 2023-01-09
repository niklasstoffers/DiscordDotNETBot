namespace Hainz.Core.Healthchecks;

public class TCPHealthCheckOptions
{
    public int InitialTimeout { get; init; }
    public int Timeout { get; init; }
    public int Interval { get; init; }
    public int UnhealthyInterval { get; init; }
}