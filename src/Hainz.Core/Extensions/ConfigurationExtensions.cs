using Hainz.Core.Config;
using Microsoft.Extensions.Configuration;

namespace Hainz.Core.Extensions;

public static class ConfigurationExtensions 
{
    public static BotConfig? GetBotConfiguration(this IConfiguration configuration) =>
        configuration.GetSection(SectionKey.Bot).Get<BotConfig>();
}