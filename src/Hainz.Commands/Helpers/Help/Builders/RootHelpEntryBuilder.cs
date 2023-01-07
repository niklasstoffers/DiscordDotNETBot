using System.Text;
using Discord;
using Discord.Commands;
using Hainz.Commands.Modules.Misc;

namespace Hainz.Commands.Helpers.Help.Builders;

public class RootHelpEntryBuilder : HelpEntryBuilderBase
{
    private readonly HelpRegister _helpRegister;
    private readonly CommandInvocationResolver _commandInvocationResolver;

    public RootHelpEntryBuilder(HelpRegister helpRegister, CommandInvocationResolver commandInvocationResolver)
    {
        _helpRegister = helpRegister;
        _commandInvocationResolver = commandInvocationResolver;
    }

    public async Task<Embed> Build(SocketCommandContext context)
    {
        var embedBuilder = CreateBaseEmbed(context);
        var contentBuilder = new StringBuilder();
        string helpCommandSearchInvocation = await _commandInvocationResolver.GetInvocation<HelpCommand>(m => m.SearchHelpAsync, context);

        contentBuilder.AppendLine($"Welcome to Hainz Help! Below is a list of topics to get you started.");
        contentBuilder.AppendLine();
        
        foreach (var section in _helpRegister.Sections)
            contentBuilder.AppendLine($"{Format.Bold(section.Name)} {section.Description}");

        contentBuilder.AppendLine();
        contentBuilder.AppendLine($"{Format.Italics("To get help for a specific topic or command, type")} {Format.Code(helpCommandSearchInvocation)}");

        embedBuilder.Description = contentBuilder.ToString();

        return embedBuilder.Build();
    }
}