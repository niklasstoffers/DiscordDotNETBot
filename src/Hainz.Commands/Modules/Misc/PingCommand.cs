using Discord.Commands;

namespace Hainz.Commands.Modules.Misc;

public sealed class PingCommand : MiscCommandBase
{
    [Command("ping")]
    public async Task PingAsync() 
    {
        await ReplyAsync("pong");
    }
}