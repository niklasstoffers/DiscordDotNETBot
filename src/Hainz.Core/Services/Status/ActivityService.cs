using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Status;

public sealed class ActivityService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(DiscordSocketClient client,
                           ILogger<ActivityService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<bool> SetGameAsync(string? game, ActivityType type = ActivityType.Playing) 
    {
        try
        {
            _logger.LogInformation("Setting game to \"{game}\"", game);
            await _client.SetGameAsync(game, type: type);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set game");
        }

        return false;
    }
}