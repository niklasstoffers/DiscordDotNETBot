using System.Diagnostics;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Bot;

namespace Hainz.Commands.Modules.Info;

[CommandName("uptime")]
[Summary("replies with the bots current uptime")]
public sealed class UptimeCommand : InfoCommandBase
{
    private readonly UptimeMonitorService _uptimeMonitorService;

    public UptimeCommand(UptimeMonitorService uptimeMonitorService)
    {
        _uptimeMonitorService = uptimeMonitorService;
    }

    [Command("uptime")]
    public async Task UptimeAsync() 
    {
        var uptimeStats = _uptimeMonitorService.UptimeStatistic;
        await ReplyAsync($"Up since {uptimeStats.CurrentUptime}");
    }
}