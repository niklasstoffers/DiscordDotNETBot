namespace Hainz.Core.Config;

public class HealthChecksConfiguration
{
    public HealthCheckConfiguration? Database { get; init; } = null;
    public HealthCheckConfiguration? Redis { get; init; } = null;
}