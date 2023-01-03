using Hainz.Config;
using Hainz.Core.Config;
using Microsoft.Extensions.Configuration;
using Hainz.Data.Configuration;
using Hainz.Data.Configuration.Caching;

namespace Hainz.Extensions;

public static class ConfigurationExtensions 
{
    public static BotConfig GetBotConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Bot).Get<BotConfig>() ?? throw new ArgumentException("Invalid bot configuration");

    public static PersistenceConfiguration GetPersistenceConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Persistence).Get<PersistenceConfiguration>() ?? throw new ArgumentException("Invalid persistence configuration");

    public static CachingConfiguration GetCachingConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Caching).Get<CachingConfiguration>() ?? throw new ArgumentException("Invalid caching configuration");

    public static PersistenceConfiguration GetPersistenceConfigurationWithEnvironmentVars(this IConfiguration configuration)
    {
        var persistenceConfiguration = configuration.GetPersistenceConfiguration();

        return new PersistenceConfiguration()
        {
            Host = EnvironmentVariable.GetPersistenceHostname() ?? persistenceConfiguration.Host,
            Port = EnvironmentVariable.GetPersistencePort() ?? persistenceConfiguration.Port,
            Password = EnvironmentVariable.GetPersistencePassword() ?? persistenceConfiguration.Password,
            Username = EnvironmentVariable.GetPersistenceUsername() ?? persistenceConfiguration.Username,
            Database = EnvironmentVariable.GetPersistenceDatabase() ?? persistenceConfiguration.Database
        };
    }

    public static CachingConfiguration GetCachingConfigurationWithEnvironmentVars(this IConfiguration configuration)
    {
        var cachingConfiguration = configuration.GetCachingConfiguration();

        return new CachingConfiguration()
        {
            CacheKeyPrefix = cachingConfiguration.CacheKeyPrefix,
            ExpirationMode = cachingConfiguration.ExpirationMode,
            ProviderName = cachingConfiguration.ProviderName,
            TimeoutSeconds = cachingConfiguration.TimeoutSeconds,
            Redis = new RedisConfiguration()
            {
                Hostname = EnvironmentVariable.GetCacheHostname() ?? cachingConfiguration.Redis.Hostname,
                Port = EnvironmentVariable.GetCachePort() ?? cachingConfiguration.Redis.Port,
                AsyncTimeout = cachingConfiguration.Redis.AsyncTimeout,
                SyncTimeout = cachingConfiguration.Redis.SyncTimeout,
                ConnectionTimeout = cachingConfiguration.Redis.ConnectionTimeout
            }
        };
    }
}