using Hainz.Data;
using Hainz.Data.Entities;

namespace Hainz.Persistence;

public sealed class DbContextAdapter : IDbContext
{
    private readonly HainzDbContext _dbContext;

    public IQueryable<GuildUser> GuildUsers => _dbContext.GuildUsers.AsQueryable();
    public IQueryable<Guild> Guilds => _dbContext.Guilds.AsQueryable();
    public IQueryable<GuildChannel> GuildChannels => _dbContext.GuildChannels.AsQueryable();

    public DbContextAdapter(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync() =>
        await _dbContext.SaveChangesAsync();
}