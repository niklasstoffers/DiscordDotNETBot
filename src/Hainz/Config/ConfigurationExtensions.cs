using Microsoft.Extensions.Configuration;

namespace Hainz.Config;

internal static class ConfigurationExtensions 
{
    public static BotConfig? GetBotConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Bot).Get<BotConfig>();
}