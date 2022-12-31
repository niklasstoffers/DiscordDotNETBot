using Hainz.Infrastructure.Logging;
using Hainz.Data.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Helpers;

public sealed class HostStartup : IDisposable
{
    private readonly IHost _host;
    private readonly IServiceScope _startupProvider;
    private readonly ILogger<HostStartup> _logger;

    public HostStartup(IHost host)
    {
        _host = host;
        _startupProvider = _host.Services.CreateScope();
        _logger = _host.Services.GetRequiredService<ILogger<HostStartup>>();
    }

    public void ReloadLogging()
    {
        _logger.LogInformation("Reloading logging with host service provider");
        LoggingServiceProviderConfigurator.ReloadConfigWithServiceProvider(_host.Services);
    }

    public async Task ApplyMigrationsAsync()
    {
        _logger.LogInformation("Applying database migrations");
        var migrationService = _startupProvider
            .ServiceProvider
            .GetRequiredService<DbMigrationHelper>();

        var numMigrationsApplied = await migrationService.ApplyMigrationsAsync();
        _logger.LogInformation("Applied migrations: {numMigrations}", numMigrationsApplied);
    }

    public void Dispose()
    {
        _startupProvider.Dispose();
    }
}