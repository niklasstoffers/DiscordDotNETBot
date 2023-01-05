using Hainz.Config;
using Hainz.Core.Config;
using Microsoft.Extensions.Configuration;
using Hainz.Data.Configuration;
using Hainz.Data.Configuration.Caching;
using Hainz.Helpers;

namespace Hainz.Extensions;

public static class ConfigurationExtensions 
{
    public static BotConfig GetBotConfiguration(this IConfiguration configuration) => 
        configuration.GetConfiguration<BotConfig>(SectionKey.Bot);
    
    public static PersistenceConfiguration GetPersistenceConfiguration(this IConfiguration configuration) => 
        configuration.GetConfiguration<PersistenceConfiguration>(
            SectionKey.Persistence, 
            opt => opt.Bind(config => config.Host, EnvironmentVariable.GetPersistenceHostname())
                      .Bind(config => config.Port, EnvironmentVariable.GetPersistencePort())
                      .Bind(config => config.Password, EnvironmentVariable.GetPersistencePassword())
                      .Bind(config => config.Username, EnvironmentVariable.GetPersistenceUsername())
                      .Bind(config => config.Database, EnvironmentVariable.GetPersistenceDatabase())
        );
    
    public static CachingConfiguration GetCachingConfiguration(this IConfiguration configuration) => 
        configuration.GetConfiguration<CachingConfiguration>(
            SectionKey.Caching,
            opt => opt.Bind(config => config.Redis.Hostname, EnvironmentVariable.GetCacheHostname())
                      .Bind(config => config.Redis.Port, EnvironmentVariable.GetCachePort())    
        );

    private static T GetConfiguration<T>(this IConfiguration configuration, string sectionName, Func<BindingOptions<T>, BindingOptions<T>>? bindingOptionsConfig = null) where T : class
    {
        var bindingOptions = new BindingOptions<T>();
        bindingOptionsConfig?.Invoke(bindingOptions);

        var sectionConfig = configuration.GetSection(sectionName).Get<T>(opt => opt.ErrorOnUnknownConfiguration = true) ?? 
                            throw new ArgumentException($"Invalid configuration for section \"{sectionName}\"");

        Binder.Bind(sectionConfig, bindingOptions);
        return sectionConfig;
    }
}