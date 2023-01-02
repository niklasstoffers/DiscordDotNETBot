using EFCoreSecondLevelCacheInterceptor;

namespace Hainz.Data.Configuration.Caching;

public sealed class CachingConfiguration
{
    public string ProviderName { get; init; } = null!;
    public string CacheKeyPrefix { get; init; } = null!;
    public CacheExpirationMode ExpirationMode { get; init; }
    public long TimeoutSeconds { get; init; }
    
    public RedisConfiguration Redis { get; init; } = null!;
}