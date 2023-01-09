using Discord;
using Discord.Commands;
using Hainz.Commands.Extensions;

namespace Hainz.Commands.Helpers.Help.Builders;

public abstract class HelpEntryBuilderBase
{
    protected static EmbedBuilder CreateBaseEmbed(SocketCommandContext context)
    {
        var embedBuilder = new EmbedBuilder();

        embedBuilder
            .WithTitle("Hainz Help!")
            .WithUser(context.Client.CurrentUser)
            .WithCurrentTimestamp();

        return embedBuilder;
    }
}