using Discord;

namespace Hainz.Core.Config.Bot;

public static class RequiredGatewayIntents
{
    public const GatewayIntents RequiredIntents = (GatewayIntents.AllUnprivileged 
            | GatewayIntents.GuildMembers 
            | GatewayIntents.MessageContent)
        & ~(GatewayIntents.GuildInvites 
            | GatewayIntents.GuildScheduledEvents);
}