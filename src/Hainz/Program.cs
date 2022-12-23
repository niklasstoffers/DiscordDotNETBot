using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hainz;
using Hainz.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

string environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
NLog.Logger buildLogger = CreateBuildLogger();

try
{
    var host = new HostBuilder()
        .UseConsoleLifetime()
        .UseEnvironment(environmentName)
        .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
        .ConfigureHostConfiguration(ConfigureHost)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureAppConfiguration(ConfigureApp)
        .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
        .ConfigureServices(ConfigureServices)
        .ConfigureLogging(ConfigureLogging)
        .Build();

    ReloadNLog(host.Services);
    await host.RunAsync();
}
catch (Exception ex) 
{
    buildLogger.Fatal(ex, "Error during startup");
}

void ConfigureHost(IConfigurationBuilder configBuilder)
{
    configBuilder.AddCommandLine(args);
}

void ConfigureApp(IConfigurationBuilder configBuilder)
{
    configBuilder.AddJsonFile("appsettings.json", optional: false);
}

void ConfigureServices(IServiceCollection services) 
{
    services.AddHostedService<Bot>();
}

void ConfigureContainer(HostBuilderContext hostContext, ContainerBuilder builder) 
{
    var assembly = Assembly.GetExecutingAssembly();
    builder.RegisterAssemblyModules(assembly);

    var botConfig = hostContext.Configuration.GetBotConfiguration() ??
                        throw new Exception("Invalid bot configuration");

    builder.Register(ctx => botConfig)
           .AsSelf()
           .SingleInstance();
}

void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder loggingBuilder) 
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);

    if (hostContext.HostingEnvironment.IsDevelopment())
    {
        loggingBuilder.AddNLog("nlog.debug.config");
    }
    else if (hostContext.HostingEnvironment.IsProduction())
    {
        loggingBuilder.AddNLog("nlog.release.config");
    }
}

NLog.Logger CreateBuildLogger() 
{
    IHostEnvironment startupEnvironment = new HostingEnvironment() 
    {
        EnvironmentName = environmentName
    };

    string nlogConfigFile = "nlog.debug.config";
    if (startupEnvironment.IsProduction())
        nlogConfigFile = "nlog.release.config";

    var buildLoggerFactory = NLog.LogManager.LoadConfiguration(nlogConfigFile);
    return buildLoggerFactory.GetCurrentClassLogger();
}

void ReloadNLog(IServiceProvider serviceProvider) 
{
    // NLog catch22 work-around. Reloads all configuration using the newly built host service provider.
    // This is required because some custom NLog targets require dependencies to work.
    // The InitializeTarget() pattern doesn't work here as well since NLog will resolve unregistered dependencies by itself if they have a default constructor ignoring the provided ServiceProvider.
    var nlogServiceProvider = NLog.Config.ConfigurationItemFactory.Default.CreateInstance;
    NLog.Config.ConfigurationItemFactory.Default.CreateInstance = 
        type => serviceProvider.GetService(type) ?? nlogServiceProvider(type);
    NLog.LogManager.Configuration = NLog.LogManager.Configuration.Reload();
}