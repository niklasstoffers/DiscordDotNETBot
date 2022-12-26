namespace Hainz.Config.Server;

public sealed class ServerConfig
{
    public string? BotAdminRole { get; init; }
    public ChannelConfig? Channels { get; init; }
}