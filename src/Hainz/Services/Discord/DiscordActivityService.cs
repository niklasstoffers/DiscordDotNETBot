using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Hainz.Services.Discord;

public class DiscordActivityService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordActivityService> _logger;

    public DiscordActivityService(DiscordSocketClient client,
                                  ILogger<DiscordActivityService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task SetGame(string? game) 
    {
        _logger.LogInformation("Setting game to \"{game}\"", game);
        await _client.SetGameAsync(game);
    }
}