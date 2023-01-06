using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public abstract class HelpEntryBuilderBase<TEntry> : HelpEntryBuilderBase where TEntry : HelpEntry
{
    protected abstract Task Fill(EmbedBuilder builder, SocketCommandContext context, TEntry entry);

    public async Task<Embed> Build(SocketCommandContext context, TEntry entry)
    {
        var embedBuilder = CreateBaseEmbed(context);
        await Fill(embedBuilder, context, entry);
        return embedBuilder.Build();
    }
}