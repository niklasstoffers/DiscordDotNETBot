using System.Diagnostics;
using Discord.Commands;

namespace Hainz.Commands.Modules;

public class InfoModule : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping() 
    {
        await Context.Channel.SendMessageAsync("pong");
    }

    [Command("uptime")]
    public async Task Uptime() 
    {
        var startupDate = Process.GetCurrentProcess().StartTime.ToUniversalTime();
        var uptime = DateTime.UtcNow - startupDate;
        await Context.Channel.SendMessageAsync($"Up since {uptime}");
    }
}