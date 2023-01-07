using Hainz.Infrastructure.Logging;
using Hainz.Data.Helpers;
using Microsoft.Extensions.Logging;
using Hainz.Config;

namespace Hainz.Helpers;

public sealed class HostStartup
{
    private readonly DbMigrationHelper _migrationHelper;
    private readonly ILogger<HostStartup> _logger;

    public HostStartup(DbMigrationHelper migrationHelper, 
                       ILogger<HostStartup> logger)
    {
        _migrationHelper = migrationHelper;
        _logger = logger;
    }

    public void ReloadLogging(IServiceProvider serviceProvider)
    {
        _logger.LogInformation("Reloading logging with host service provider");
        LoggingServiceProviderConfigurator.ReloadConfigWithServiceProvider(serviceProvider);
    }

    public async Task ApplyMigrationsAsync()
    {
        if (EnvironmentVariable.GetPreventMigrations())
        {
            _logger.LogInformation("Skipping database migrations because prevent migrations is enabled");
        }
        else
        {
            _logger.LogInformation("Applying database migrations");
            var numMigrationsApplied = await _migrationHelper.ApplyMigrationsAsync();
            _logger.LogInformation("Applied migrations: {numMigrations}", numMigrationsApplied);
        }
    }
}