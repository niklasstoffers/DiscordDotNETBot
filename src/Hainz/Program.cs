using Autofac.Extensions.DependencyInjection;
using Hainz.Config;
using Hainz.Extensions;
using Hainz.Helpers;
using Hainz.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

string environment = EnvironmentVariable.GetDotNetEnvironment();
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

    using var hostStartup = new HostStartup(host);
    hostStartup.ReloadLogging();
    await hostStartup.ApplyMigrationsAsync();

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