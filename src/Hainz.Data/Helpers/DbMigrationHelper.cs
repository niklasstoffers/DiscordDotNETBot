using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Helpers;

public class DbMigrationHelper
{
    private readonly HainzDbContext _dbContext;

    public DbMigrationHelper(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> ApplyMigrationsAsync()
    {
        var numPendingMigrations = (await _dbContext.Database.GetPendingMigrationsAsync()).Count();
        await _dbContext.Database.MigrateAsync();
        return numPendingMigrations;
    }
}