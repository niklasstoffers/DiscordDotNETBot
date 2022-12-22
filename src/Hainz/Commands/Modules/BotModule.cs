using Discord.Commands;
using Discord.WebSocket;
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
    public async Task SetGame(params string[] game) 
    {
        var gameName = string.Join(" ", game);
        await _activityService.SetGame(gameName);
    }

    [Command("setstatus")]
    public async Task SetStatus(string status) 
    {
        if (!await _statusService.SetStatus(status)) 
        {
            await Context.Channel.SendMessageAsync("Unknown status");
            _logger.LogWarning("Received invalid status argument \"{status}\" in setstatus command", status);
        }
    }
}