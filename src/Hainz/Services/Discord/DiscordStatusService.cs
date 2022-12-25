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

    public async Task<bool> SetStatusAsync(string? status) 
    {
        UserStatus? userStatus = null;

        if (status != null)
            userStatus = UserStatusMap.UserStatusFromString(status);

        if (userStatus == null) 
        {
            _logger.LogWarning("Invalid user status. Tried to set status to \"{status}\"", status);
        }
        else 
        {
            _logger.LogInformation("Setting status to \"{status}\"", status);
            await _client.SetStatusAsync(userStatus.Value);
            return true;
        }

        return false;
    }
}