using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(ConfigureApp)
    .ConfigureServices(ConfigureServices)
    .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
    .ConfigureLogging(ConfigureLogging)
    .RunConsoleAsync();

void ConfigureApp(IConfigurationBuilder configBuilder)
{
    configBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
    configBuilder.AddJsonFile("appsettings.json", optional: false);
}

void ConfigureServices(IServiceCollection services) 
{
    services.AddCore();
    services.AddCommands();
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
    loggingBuilder.AddNLog("nlog.config");
}