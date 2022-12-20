using Discord.Commands;
using Discord.WebSocket;
using Hainz.Core.Services.Discord;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules;

public class BotModule : ModuleBase<SocketCommandContext>
{
    private DiscordSocketClient _client;
    private DiscordStatusService _statusService;
    private ILogger<BotModule> _logger;

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
    }

    [Command("setstatus")]
    public async Task SetStatus(string status) 
    {
        if (!await _statusService.SetStatus(status))
            await Context.Channel.SendMessageAsync("Unknown status");
    }
}