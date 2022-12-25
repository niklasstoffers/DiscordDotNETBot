using Hainz.Config.Logging;

namespace Hainz.Config;

public sealed class BotConfig 
{
#pragma warning disable CS8618
    public string Token { get; init; }
    public string StatusGameName { get; init; }
    public string Status { get; init; }
    public LoggingConfig Logging { get; init; }
#pragma warning restore CS8618
}