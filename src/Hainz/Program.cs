using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hainz;
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
    configBuilder.AddJsonFile("appsettings.json", optional: true);
}

void ConfigureServices(IServiceCollection services) 
{
    services.AddHostedService<Bot>();
}

void ConfigureContainer(ContainerBuilder builder) 
{
    var assembly = Assembly.GetExecutingAssembly();
    builder.RegisterAssemblyModules(assembly);
}

void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder loggingBuilder) 
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.AddNLog("nlog.config");
}