using Autofac.Extensions.DependencyInjection;
using Hainz.Extensions;
using Hainz.Infrastructure;
using Hainz.Logging;
using Microsoft.Extensions.Hosting;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
var startupEnvironment = new StartupEnvironment(environment);
var rootLogger = startupEnvironment.Logger;

try
{
    var host = new HostBuilder()
        .UseConsoleLifetime()
        .UseEnvironment(environment)
        .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .RegisterAutofacServices()
        .AddAppSettings(false)
        .AddNLogConfiguration()
        .AddApplicationConfiguration()
        .AddAutoMapper()
        .AddMediatR()
        .AddEvents()
        .AddApplicationHost()
        .Build();

    NLogServiceProviderConfigurator.ReloadConfigWithServiceProvider(host.Services);

    try 
    {
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