using System.Reflection;
using Autofac;
using FluentValidation;
using Hainz.Config.Validation;
using Hainz.Events.Extensions;
using MediatR;
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

    public static IHostBuilder AddApplicationConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) => 
        {
            var botConfiguration = hostBuilder.Configuration.GetBotConfiguration();
            var serverConfiguration = hostBuilder.Configuration.GetServerConfiguration();

            var botConfigurationValidator = new BotConfigValidator();
            botConfigurationValidator.ValidateAndThrow(botConfiguration);

            serviceCollection.AddSingleton(botConfiguration);
            serviceCollection.AddSingleton(serverConfiguration);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddAutoMapper(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            serviceCollection.AddAutoMapper(currentAssembly);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddMediatR(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            serviceCollection.AddMediatR(currentAssembly);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddEvents(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            serviceCollection.AddEvents();
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