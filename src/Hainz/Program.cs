using Autofac.Extensions.DependencyInjection;
using Hainz.Extensions;
using Hainz.Infrastructure;
using Hainz.Logging.NLog;
using Microsoft.Extensions.Hosting;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
var startupEnvironment = new StartupEnvironment(environment);
var rootLogger = startupEnvironment.Logger;

try
{
    rootLogger.Info("Starting host building");
    var host = new HostBuilder()
        .UseConsoleLifetime()
        .UseEnvironment(environment)
        .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .AddServices()
        .AddAppSettings(false)
        .AddNLog()
        .AddApplicationConfiguration()
        .AddApplicationHost()
        .Build();

    rootLogger.Info("Host building complete");

    rootLogger.Info("Reloading NLog with host service provider");
    NLogServiceProviderConfigurator.ReloadConfigWithServiceProvider(host.Services);

    try 
    {
        rootLogger.Info("Starting host");
        await host.RunAsync();
    }
    catch (Exception ex)
    {
        rootLogger.Fatal(ex, "Error occured during host execution");
    }
}
catch (Exception ex) 
{
    rootLogger.Fatal(ex, "Error occured during host building");
}