using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Services.Guild;

public class GuildService
{
    private readonly HainzDbContext _dbContext;

    public GuildService(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Entities.Guild> GetOrCreateByDiscordId(ulong guildId)
    {
        var guild = await _dbContext.Guilds.SingleOrDefaultAsync(guild => guild.DiscordGuildId == guildId);
        
        if (guild == null)
        {
            guild = new()
            {
                DiscordGuildId = guildId
            };

            _dbContext.Guilds.Add(guild);
            await _dbContext.SaveChangesAsync();
        }

        return guild;
    }
}