using Hainz.Core.Config;
using Hainz.Data.Configuration.Caching;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Healthchecks.Services;

public class RedisHealthCheck : RemoteServiceHealthCheckBase
{
    private readonly ILogger<RedisHealthCheck> _logger;

    public RedisHealthCheck(HealthChecksConfiguration config,
                            CachingConfiguration cachingConfiguration,
                            ILogger<RedisHealthCheck> logger) : base(cachingConfiguration.Redis.Hostname, cachingConfiguration.Redis.Port, config.Redis)
    {
        _logger = logger;   
    }

    public override async Task StartAsync()
    {
        if (IsEnabled)
        {
            _logger.LogInformation("Starting TCP healthcheck for redis");
            await TCPHealthCheck.StartAsync();
        }
        else
        {
            _logger.LogInformation("Redis health check disabled");
        }
    }

    public override async Task StopAsync()
    {
        _logger.LogInformation("Stopping healthcheck for redis");
        await TCPHealthCheck.StopAsync();
    }
}