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