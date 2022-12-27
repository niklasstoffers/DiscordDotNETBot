using System.Diagnostics;
using Discord.Commands;

namespace Hainz.Commands.Modules.Info;

public sealed class UptimeCommand : InfoCommandBase
{
    [Command("uptime")]
    public async Task UptimeAsync() 
    {
        var startupDate = Process.GetCurrentProcess().StartTime.ToUniversalTime();
        var uptime = DateTime.UtcNow - startupDate;
        await Context.Channel.SendMessageAsync($"Up since {uptime}");
    }
}