using Discord;
using Discord.WebSocket;
using Hainz.Helpers.Discord;
using Microsoft.Extensions.Logging;

namespace Hainz.Services.Discord;

public sealed class DiscordStatusService 
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordStatusService> _logger;

    public DiscordStatusService(DiscordSocketClient client,
                                ILogger<DiscordStatusService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<bool> SetStatusAsync(UserStatus status) 
    {
        try
        {
            _logger.LogInformation("Setting status to \"{status}\"", status);
            await _client.SetStatusAsync(status);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set status");
        }

        return false;
    }
}