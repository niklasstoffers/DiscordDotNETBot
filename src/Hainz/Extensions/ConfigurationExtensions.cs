using Hainz.Config;
using Hainz.Config.Bot;
using Hainz.Config.Server;
using Microsoft.Extensions.Configuration;

namespace Hainz.Extensions;

public static class ConfigurationExtensions 
{
    public static BotConfig GetBotConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Bot).Get<BotConfig>() ?? throw new ArgumentException("Invalid bot configuration");

    public static ServerConfig GetServerConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Server).Get<ServerConfig>() ?? throw new ArgumentException("Invalid server configuration");
}