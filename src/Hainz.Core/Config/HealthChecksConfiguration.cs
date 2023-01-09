namespace Hainz.Core.Config;

public class HealthChecksConfiguration
{
    public HealthCheckConfiguration? Database { get; init; }
    public HealthCheckConfiguration? Redis { get; init; }
}