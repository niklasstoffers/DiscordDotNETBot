using Hainz.Infrastructure.Logging;
using Hainz.Data.Helpers;
using Microsoft.Extensions.Logging;

namespace Hainz.Helpers;

public sealed class HostStartup
{
    private readonly DbMigrationHelper _migrationHelper;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<HostStartup> _logger;

    public HostStartup(DbMigrationHelper migrationHelper, 
                       IServiceProvider serviceProvider, 
                       ILogger<HostStartup> logger)
    {
        _migrationHelper = migrationHelper;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void ReloadLogging()
    {
        _logger.LogInformation("Reloading logging with host service provider");
        LoggingServiceProviderConfigurator.ReloadConfigWithServiceProvider(_serviceProvider);
    }

    public async Task ApplyMigrationsAsync()
    {
        _logger.LogInformation("Applying database migrations");
        var numMigrationsApplied = await _migrationHelper.ApplyMigrationsAsync();
        _logger.LogInformation("Applied migrations: {numMigrations}", numMigrationsApplied);
    }
}