using Discord.Commands;
using Hainz.Services.Discord;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules;

public class BotModule : ModuleBase<SocketCommandContext>
{
    private readonly DiscordStatusService _statusService;
    private readonly DiscordActivityService _activityService;
    private readonly ILogger<BotModule> _logger;

    public BotModule(DiscordStatusService statusService,
                     DiscordActivityService activityService,
                     ILogger<BotModule> logger) 
    {
        _statusService = statusService;
        _activityService = activityService;
        _logger = logger;
    }

    [Command("setgame")]
    public async Task SetGameAsync(params string[] game) 
    {
        var gameName = string.Join(" ", game);
        await _activityService.SetGameAsync(gameName);
        await Context.Channel.SendMessageAsync($"Set game to \"{gameName}\"");
    }

    [Command("setstatus")]
    public async Task SetStatusAsync(string status) 
    {
        if (await _statusService.SetStatusAsync(status)) 
        {
            await Context.Channel.SendMessageAsync($"Set status to \"{status}\"");
        }
        else
        {
            await Context.Channel.SendMessageAsync("Unknown status");
            _logger.LogWarning("Received invalid status argument \"{status}\" in setstatus command", status);
        }
    }
}