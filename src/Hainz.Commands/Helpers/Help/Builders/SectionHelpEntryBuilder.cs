using System.Text;
using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public class SectionHelpEntryBuilder : HelpEntryBuilderBase<SectionHelpEntry>
{
    private readonly HelpCommandInvocationResolver _helpCommandInvocationResolver;

    public SectionHelpEntryBuilder(HelpCommandInvocationResolver helpCommandInvocationResolver)
    {
        _helpCommandInvocationResolver = helpCommandInvocationResolver;
    }

    protected override async Task Fill(EmbedBuilder builder, SocketCommandContext context, SectionHelpEntry entry)
    {
        var contentBuilder = new StringBuilder();
        var helpSearchInvocation = await _helpCommandInvocationResolver.GetSearchInvocation(context.Channel);
        contentBuilder.AppendLine($"Below is a list of commands for \"{entry.Name}\". To get help for a specific command, type {Format.Code(helpSearchInvocation)}");
        contentBuilder.AppendLine();

        foreach (var command in entry.Commands)
            contentBuilder.AppendLine($"{Format.Bold(command.Name)} {command.Description}");

        builder.Description = contentBuilder.ToString();
    }
}