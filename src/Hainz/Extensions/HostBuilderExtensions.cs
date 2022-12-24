using System.Reflection;
using Autofac;
using Hainz.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Hainz.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddNLogConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging((hostBuilderContext, loggingBuilder) => 
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);

            if (hostBuilderContext.HostingEnvironment.IsDevelopment())
            {
                loggingBuilder.AddNLog("nlog.debug.config");
            }
            else if (hostBuilderContext.HostingEnvironment.IsProduction())
            {
                loggingBuilder.AddNLog("nlog.release.config");
            }
        });

        return hostBuilder;
    }

    public static IHostBuilder RegisterAutofacServices(this IHostBuilder hostBuilder) 
    {
        hostBuilder.ConfigureContainer<ContainerBuilder>((hostContext, containerBuilder) => 
        {
            var assembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyModules(assembly);

            var botConfig = hostContext.Configuration.GetBotConfiguration() ??
                                throw new Exception("Invalid bot configuration");

            containerBuilder.Register(ctx => botConfig)
                .AsSelf()
                .SingleInstance();
        });

        return hostBuilder;
    }

    public static IHostBuilder AddAppSettings(this IHostBuilder hostBuilder, bool optional = true)
    {
        hostBuilder.ConfigureAppConfiguration(configurationBuilder => 
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: optional);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddApplicationHost(this IHostBuilder hostBuilder) 
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) => 
        {
            serviceCollection.AddHostedService<ApplicationHost>();
        });

        return hostBuilder;
    }
}