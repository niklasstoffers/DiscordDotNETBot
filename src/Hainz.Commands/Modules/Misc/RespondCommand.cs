using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules.Misc;

[CommandName("respond")]
[Summary("responds with a specified message")]
public class RespondCommand : MiscCommandBase
{
    [Command("respond")]
    public async Task RespondAsync([Remainder, CommandParameter("message", "the message with which to respond")] string message)
    {
        await ReplyAsync(message);
    }
}