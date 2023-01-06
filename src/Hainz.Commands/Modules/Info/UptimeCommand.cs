using System.Diagnostics;
using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules.Info;

[CommandName("uptime")]
[Summary("replies with the bots current uptime")]
public sealed class UptimeCommand : InfoCommandBase
{
    [Command("uptime")]
    public async Task UptimeAsync() 
    {
        var startupDate = Process.GetCurrentProcess().StartTime.ToUniversalTime();
        var uptime = DateTime.UtcNow - startupDate;
        await ReplyAsync($"Up since {uptime}");
    }
}