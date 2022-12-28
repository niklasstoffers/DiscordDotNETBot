using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.User;

public sealed class BanService
{
    private readonly ILogger<BanService> _logger;

    public BanService(ILogger<BanService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> BanAsync(SocketGuildUser user)
    {
        try
        {
            await user.BanAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while banning user");
            return false;
        }
    }

    public async Task<bool> BanAsync(SocketGuild guild, ulong userId)
    {
        try
        {
            await guild.AddBanAsync(userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while banning user");
            return false;
        }
    }

    public async Task<bool> UnbanAsync(SocketGuild guild, ulong userId)
    {
        try
        {
            await guild.RemoveBanAsync(userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to unban user");
            return false;
        }
    }
}