using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public class CommandHelpEntryBuilder : HelpEntryBuilderBase<CommandHelpEntry>
{
    protected override Task Fill(EmbedBuilder builder, SocketCommandContext context, CommandHelpEntry entry)
    {
        throw new NotImplementedException();
    }
}