using Microsoft.EntityFrameworkCore;
using Hainz.Data.Entities;

namespace Hainz.Data;

public sealed class HainzDbContext : DbContext
{
    private readonly DbInitializer _dbInitializer;

    public DbSet<Guild> Guilds { get; set; } = null!;
    public DbSet<GuildSetting> GuildSettings { get; set; } = null!;
    public DbSet<Channel> Channels { get; set; } = null!;
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; } = null!;

    public HainzDbContext(DbContextOptions<HainzDbContext> options, DbInitializer dbInitializer) : base(options) 
    {
        _dbInitializer = dbInitializer;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _dbInitializer.Seed(modelBuilder);
    }
}