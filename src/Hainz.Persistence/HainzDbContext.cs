using Microsoft.EntityFrameworkCore;
using Hainz.Data.Entities;

namespace Hainz.Persistence;

public sealed class HainzDbContext : DbContext
{
    public DbSet<Guild> Guilds { get; set; } = null!;
    public DbSet<GuildChannel> GuildChannels { get; set; } = null!;
    public DbSet<GuildUser> GuildUsers { get; set; } = null!;

    public HainzDbContext(DbContextOptions<HainzDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}