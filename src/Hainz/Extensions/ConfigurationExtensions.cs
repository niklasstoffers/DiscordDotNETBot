using Hainz.Config;
using Microsoft.Extensions.Configuration;

namespace Hainz.Extensions;

public static class ConfigurationExtensions 
{
    public static BotConfig? GetBotConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Bot).Get<BotConfig>();
}