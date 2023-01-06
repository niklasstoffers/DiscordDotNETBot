using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public class HelpEntryBuilder
{
    private readonly RootHelpEntryBuilder _rootHelpBuilder;
    private readonly SectionHelpEntryBuilder _sectionHelpEntryBuilder;
    private readonly CommandHelpEntryBuilder _commandHelpEntryBuilder;

    public HelpEntryBuilder(RootHelpEntryBuilder rootHelpBuilder,
                            SectionHelpEntryBuilder sectionHelpBuilder,
                            CommandHelpEntryBuilder commandHelpEntryBuilder)
    {
        _rootHelpBuilder = rootHelpBuilder;
        _sectionHelpEntryBuilder = sectionHelpBuilder;
        _commandHelpEntryBuilder = commandHelpEntryBuilder;
    }

    public async Task<Embed> GetRootHelp(SocketCommandContext context) => await _rootHelpBuilder.Build(context);

    public async Task<Embed> GetHelp(HelpEntry helpEntry, SocketCommandContext context)
    {
        return helpEntry switch
        {
            SectionHelpEntry sectionEntry => await _sectionHelpEntryBuilder.Build(context, sectionEntry),
            CommandHelpEntry commandEntry => await _commandHelpEntryBuilder.Build(context, commandEntry),
            _ => throw new ArgumentException($"Unknown help entry type \"{helpEntry.GetType().Name}\"")
        };
    }
}