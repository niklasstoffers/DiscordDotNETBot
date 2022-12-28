using Discord.Commands;

namespace Hainz.Commands.Modules.Misc;

public sealed class HelpCommand : MiscCommandBase
{
    [Command("help")]
    [Summary("Responds with the bots command list")]
    public async Task HelpAsync()
    {
        
    }
}