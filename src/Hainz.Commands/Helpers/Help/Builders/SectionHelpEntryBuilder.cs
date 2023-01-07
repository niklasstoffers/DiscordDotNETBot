using System.Text;
using Discord;
using Discord.Commands;
using Hainz.Commands.Modules.Misc;

namespace Hainz.Commands.Helpers.Help.Builders;

public class SectionHelpEntryBuilder : HelpEntryBuilderBase<SectionHelpEntry>
{
    private readonly CommandInvocationResolver _commandInvocationResolver;

    public SectionHelpEntryBuilder(CommandInvocationResolver commandInvocationResolver)
    {
        _commandInvocationResolver = commandInvocationResolver;
    }

    protected override async Task Fill(EmbedBuilder builder, SocketCommandContext context, SectionHelpEntry entry)
    {
        var contentBuilder = new StringBuilder();
        var helpSearchInvocation = await _commandInvocationResolver.GetInvocation<HelpCommand>(m => m.SearchHelpAsync, context);
        contentBuilder.AppendLine(Format.Underline(entry.Description));
        contentBuilder.AppendLine();

        foreach (var command in entry.Commands)
            contentBuilder.AppendLine($"{Format.Bold(command.Name)} {command.Description}");

        contentBuilder.AppendLine();
        contentBuilder.Append($"{Format.Italics("To get help for a specific command, type")} {Format.Code(helpSearchInvocation)}");

        builder.Description = contentBuilder.ToString();
    }
}