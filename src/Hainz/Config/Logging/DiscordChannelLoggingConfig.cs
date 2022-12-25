namespace Hainz.Config.Logging;

public sealed class DiscordChannelLoggingConfig 
{
    public bool IsEnabled { get; init; }
    public ulong LogChannelId { get; init; }
}