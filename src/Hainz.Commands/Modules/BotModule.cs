using Discord.Commands;
using Discord.WebSocket;
using Hainz.Core.Services.Discord;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules;

public class BotModule : ModuleBase<SocketCommandContext>
{
    private readonly DiscordSocketClient _client;
    private readonly DiscordStatusService _statusService;
    private readonly ILogger<BotModule> _logger;

    public BotModule(DiscordSocketClient client,
                     DiscordStatusService statusService,
                     ILogger<BotModule> logger) 
    {
        _client = client;
        _statusService = statusService;
        _logger = logger;
    }

    [Command("setgame")]
    public async Task SetGame(params string[] game) 
    {
        var gameName = string.Join(" ", game);
        await _client.SetGameAsync(gameName);
        _logger.LogInformation("Setting game to \"{game}\"", game);
    }

    [Command("setstatus")]
    public async Task SetStatus(string status) 
    {
        if (!await _statusService.SetStatus(status)) 
        {
            await Context.Channel.SendMessageAsync("Unknown status");
            _logger.LogWarning("Received invalid status argument \"{status}\"", status);
        }
        else 
        {
            _logger.LogInformation("Setting status to \"{status}\"", status);
        }
    }
}