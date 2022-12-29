using Autofac.Extensions.DependencyInjection;
using Hainz.Extensions;
using Hainz.Infrastructure;
using Hainz.Infrastructure.Logging;
using Hainz.Persistence.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
StartupEnvironment startupEnvironment = new(environment);
ILogger rootLogger = startupEnvironment.Logger;

try
{
    rootLogger.LogInformation("Starting host building");
    var host = new HostBuilder()
        .UseConsoleLifetime()
        .UseEnvironment(environment)
        .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .AddAppSettings(false)
        .AddApplicationConfiguration()
        .AddServices()
        .AddApplicationHost()
        .Build();

    ReloadLogging(host);
    await ApplyMigrations(host);

    try 
    {
        rootLogger.LogInformation("Starting host");
        await host.RunAsync();
    }
    catch (Exception ex)
    {
        rootLogger.LogCritical(ex, "Error occured during host execution");
    }
}
catch (Exception ex) 
{
    rootLogger.LogCritical(ex, "Error occured during host building");
}

void ReloadLogging(IHost host)
{
    rootLogger.LogInformation("Reloading logging with host service provider");
    LoggingServiceProviderConfigurator.ReloadConfigWithServiceProvider(host.Services);
}

async Task ApplyMigrations(IHost host)
{
    rootLogger.LogInformation("Applying database migrations");
    using var migrationServiceScope = host.Services.CreateScope();
    var migrationService = migrationServiceScope
        .ServiceProvider
        .GetRequiredService<DbMigrationHelper>();

    var numMigrationsApplied = await migrationService.ApplyMigrationsAsync();

    rootLogger.LogInformation("Applied Migrations: {numMigrations}", numMigrationsApplied);
}