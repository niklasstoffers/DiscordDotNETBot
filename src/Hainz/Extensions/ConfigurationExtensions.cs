using Hainz.Config;
using Hainz.Core.Config.BotOptions;
using Hainz.Core.Config.Bot;
using Hainz.Core.Config.Server;
using Microsoft.Extensions.Configuration;
using Hainz.Persistence.Configuration;

namespace Hainz.Extensions;

public static class ConfigurationExtensions 
{
    public static BotConfig GetBotConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Bot).Get<BotConfig>() ?? throw new ArgumentException("Invalid bot configuration");

    public static ServerConfig GetServerConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Server).Get<ServerConfig>() ?? throw new ArgumentException("Invalid server configuration");

    public static BotOptionsConfig GetBotOptionsConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.BotOptions).Get<BotOptionsConfig>() ?? throw new ArgumentException("Invalid bot options configuration");

    public static PersistenceConfiguration GetPersistenceConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Persistence).Get<PersistenceConfiguration>() ?? throw new ArgumentException("Invalid persistence configuration");

    public static PersistenceConfiguration GetPersistenceConfigurationWithEnvVars(this IConfiguration configuration)
    {
        var appsettingsPersistenceConfiguration = configuration.GetPersistenceConfiguration();

        var persistenceHostname = Environment.GetEnvironmentVariable("PERSISTENCE_HOSTNAME");
        var persistencePort = Environment.GetEnvironmentVariable("PERSISTENCE_PORT");
        var persistenceUsername = Environment.GetEnvironmentVariable("PERSISTENCE_USERNAME");
        var persistencePassword = Environment.GetEnvironmentVariable("PERSISTENCE_PASSWORD");
        var persistenceDb = Environment.GetEnvironmentVariable("PERSISTENCE_DB");

        int? port = null;
        if (!string.IsNullOrEmpty(persistencePort))
            port = int.Parse(persistencePort);

        return new PersistenceConfiguration()
        {
            Host = persistenceHostname ?? appsettingsPersistenceConfiguration.Host,
            Port = port ?? appsettingsPersistenceConfiguration.Port,
            Password = persistencePassword ?? appsettingsPersistenceConfiguration.Password,
            Username = persistenceUsername ?? appsettingsPersistenceConfiguration.Username,
            Database = persistenceDb ?? appsettingsPersistenceConfiguration.Database
        };
    }
}