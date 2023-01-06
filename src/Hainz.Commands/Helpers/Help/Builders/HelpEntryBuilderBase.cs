using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public abstract class HelpEntryBuilderBase
{
    protected static EmbedBuilder CreateBaseEmbed(SocketCommandContext context)
    {
        var embedBuilder = new EmbedBuilder();

        embedBuilder
            .WithTitle("Hainz Help!")
            .WithFooter(builder =>
            {
                builder.Text = context.Client.CurrentUser.Username;
                builder.IconUrl = context.Client.CurrentUser.GetAvatarUrl();
            })
            .WithCurrentTimestamp();

        return embedBuilder;
    }
}