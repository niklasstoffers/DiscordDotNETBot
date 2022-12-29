using Discord;
using Discord.WebSocket;
using Hainz.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Messages;

public sealed class DMService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DMService> _logger;

    public DMService(DiscordSocketClient client,
                     ILogger<DMService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<bool> SendDMAsync(ulong userId, string message) 
    {
        var user = await _client.GetUserAsync(userId);

        if (user == null) 
        {
            _logger.LogWarning("Cannot send DM to user. Unknown user id {id}", userId);
            return false;
        }
        else 
        {
            return await SendDMAsync(user, message);
        }
    }

    public async Task<bool> SendDMAsync(IUser user, string message)
    {
        return await TryWrapper.TryAsync(
            async () => await user.SendMessageAsync(message), 
            ex => _logger.LogError(ex, "Exception while trying to send DM to user with id {id}", user.Id)
        );
    }
}