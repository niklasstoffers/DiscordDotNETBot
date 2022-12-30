using Hainz.Data.Entities;

namespace Hainz.Data;

public interface IDbContext
{
    IQueryable<Guild> Guilds { get; }
    IQueryable<GuildChannel> GuildChannels { get; }
    IQueryable<GuildUser> GuildUsers { get; }

    Task SaveChangesAsync();
}