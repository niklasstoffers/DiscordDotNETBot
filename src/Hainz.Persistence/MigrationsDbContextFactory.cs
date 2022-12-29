using Hainz.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hainz.Persistence;

public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<HainzDbContext>
{
    public HainzDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var persistenceConfiguration = configuration.GetSection("Hainz:Persistence").Get<PersistenceConfiguration>() ?? throw new Exception("Missing persistence configuration");
        var connectionString = persistenceConfiguration.ToConnectionString();

        var builder = new DbContextOptionsBuilder<HainzDbContext>();
        builder.UseNpgsql(connectionString);

        return new HainzDbContext(builder.Options);
    }
}