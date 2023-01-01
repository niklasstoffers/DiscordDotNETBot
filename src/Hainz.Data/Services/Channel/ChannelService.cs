using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Services.Channel;

public class ChannelService
{
    private readonly HainzDbContext _dbContext;

    public ChannelService(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Entities.Channel> GetOrCreateByDiscordId(ulong channelId)
    {
        var channel = await _dbContext.Channels
            .SingleOrDefaultAsync(channel => channel.DiscordChannelId == channelId);

        if (channel == null)
        {
            channel = new()
            {
                DiscordChannelId = channelId
            };

            _dbContext.Channels.Add(channel);
            await _dbContext.SaveChangesAsync();
        }

        return channel;
    }
}