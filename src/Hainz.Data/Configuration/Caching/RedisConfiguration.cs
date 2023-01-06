namespace Hainz.Data.Configuration.Caching;

public sealed class RedisConfiguration
{
    public string Hostname { get; init; } = null!;
    public int Port { get; init; }
    public int SyncTimeout { get; init; }
    public int AsyncTimeout { get; init; }
    public int ConnectionTimeout { get; init; }
}