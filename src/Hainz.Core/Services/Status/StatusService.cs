using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Status;

public sealed class StatusService 
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<StatusService> _logger;

    public StatusService(DiscordSocketClient client,
                         ILogger<StatusService> logger)
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