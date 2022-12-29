using Microsoft.EntityFrameworkCore;

namespace Hainz.Persistence;

public sealed class HainzDbContext : DbContext
{
    public HainzDbContext(DbContextOptions<HainzDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}