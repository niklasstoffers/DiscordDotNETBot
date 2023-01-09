using Hainz.Core.Config;
using Hainz.Data.Configuration;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Healthchecks.Services;

public class DatabaseHealthCheck : RemoteServiceHealthCheckBase
{
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(HealthChecksConfiguration config, 
                               PersistenceConfiguration persistenceConfig,
                               ILogger<DatabaseHealthCheck> logger) : base(persistenceConfig.Host, persistenceConfig.Port, config.Database) 
    {
        _logger = logger;
    }

    public override async Task StartAsync()
    {
        if (IsEnabled)
        {
            _logger.LogInformation("Starting TCP healthcheck for database");
            await TCPHealthCheck.StartAsync();
        }
        else
        {
            _logger.LogInformation("Database health check disabled");
        }
    }

    public override async Task StopAsync()
    {
        _logger.LogInformation("Stopping healthcheck for database");
        await TCPHealthCheck.StopAsync();
    }
}