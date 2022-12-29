using Autofac.Extensions.DependencyInjection;
using Hainz.Extensions;
using Hainz.Infrastructure;
using Hainz.Infrastructure.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
var startupEnvironment = new StartupEnvironment(environment);
var rootLogger = startupEnvironment.Logger;

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

    rootLogger.LogInformation("Reloading logging with host service provider");
    LoggingServiceProviderConfigurator.ReloadConfigWithServiceProvider(host.Services);

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