using Discord;
using Discord.WebSocket;
using Hainz.Commands.Extensions;

namespace Hainz.Commands.Embeds;

public class DefaultEmbedTemplateBuilder
{
    private readonly DiscordSocketClient _client;

    public DefaultEmbedTemplateBuilder(DiscordSocketClient client)
    {
        _client = client;
    }

    public EmbedBuilder Build()
    {
        return new EmbedBuilder()
            .WithUser(_client.CurrentUser)
            .WithCurrentTimestamp();
    }
}