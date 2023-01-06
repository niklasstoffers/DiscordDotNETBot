using System.Text;
using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public class RootHelpEntryBuilder : HelpEntryBuilderBase
{
    private readonly HelpRegister _helpRegister;
    private readonly HelpCommandInvocationResolver _helpCommandInvocationResolver;

    public RootHelpEntryBuilder(HelpRegister helpRegister, HelpCommandInvocationResolver helpCommandInvocationResolver)
    {
        _helpRegister = helpRegister;
        _helpCommandInvocationResolver = helpCommandInvocationResolver;
    }

    public async Task<Embed> Build(SocketCommandContext context)
    {
        var embedBuilder = CreateBaseEmbed(context);
        var contentBuilder = new StringBuilder();
        string helpCommandSearchInvocation = await _helpCommandInvocationResolver.GetSearchInvocation(context.Channel);

        contentBuilder.AppendLine($"Below is a list of command sections. To get help for a specific section or command, type {Format.Code(helpCommandSearchInvocation)}");
        contentBuilder.AppendLine();
        
        foreach (var section in _helpRegister.Sections)
            contentBuilder.AppendLine($"{Format.Bold(section.Name)} {section.Description}");

        embedBuilder.Description = contentBuilder.ToString();

        return embedBuilder.Build();
    }
}