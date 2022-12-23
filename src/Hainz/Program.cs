using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hainz;
using Hainz.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

await new HostBuilder()
    .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
    .ConfigureHostConfiguration(ConfigureHost)
    .UseEnvironment(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production")
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(ConfigureApp)
    .ConfigureServices(ConfigureServices)
    .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
    .ConfigureLogging(ConfigureLogging)
    .RunConsoleAsync();

void ConfigureHost(IConfigurationBuilder configBuilder)
{
    configBuilder.AddEnvironmentVariables("DOTNET_");
}

void ConfigureApp(IConfigurationBuilder configBuilder)
{
    configBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
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