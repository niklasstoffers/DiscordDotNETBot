using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules.Misc;

[CommandName("ping")]
[Summary("replies with a message")]
public sealed class PingCommand : MiscCommandBase
{
    [Command("ping")]
    public async Task PingAsync() 
    {
        await ReplyAsync("pong");
    }
}