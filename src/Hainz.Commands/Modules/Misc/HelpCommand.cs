using Discord.Commands;
using Hainz.Commands.Helpers.Help;
using Hainz.Commands.Helpers.Help.Builders;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules.Misc;

[CommandName("help")]
[Summary("bot help")]
public sealed class HelpCommand : MiscCommandBase
{
    private readonly HelpRegister _helpRegister;
    private readonly HelpEntryBuilder _helpEntryBuilder;

    public HelpCommand(HelpRegister helpRegister, HelpEntryBuilder helpEntryBuilder)
    {
        _helpRegister = helpRegister;
        _helpEntryBuilder = helpEntryBuilder;
    }

    [Command("help")]
    public async Task HelpAsync()
    {
        var embed = await _helpEntryBuilder.GetRootHelp(Context);
        await ReplyAsync(embed: embed);
    }

    [Command("help")]
    public async Task HelpAsync([Remainder, CommandParameter("search", "search term")] string search)
    {
        var helpEntry = _helpRegister.GetEntry(search, out var wasFuzzyMatch);

        if (helpEntry == null)
        {
            await ReplyAsync($"No help for \"{search}\" found");
        }
        else if (wasFuzzyMatch)
        {
            await ReplyAsync($"No help for \"{search}\" found. Did you mean \"{helpEntry.Name}\"?");
        }
        else
        {
            var embed = await _helpEntryBuilder.GetHelp(helpEntry, Context);
            await ReplyAsync(embed: embed);
        }
    }
}