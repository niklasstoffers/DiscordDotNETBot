using Discord;
using Discord.WebSocket;
using FluentValidation;
using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Hainz.Core.Validation.Configuration;
using Hainz.Events.Extensions;
using Hainz.Logging.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Hainz.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddNLog(this IHostBuilder hostBuilder)
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

    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            serviceCollection.AddEvents();
            serviceCollection.AddCore();
            serviceCollection.AddLoggingServices();
            serviceCollection.AddCommands();

            serviceCollection.AddSingleton<Bot>();
            serviceCollection.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
            {
                GatewayIntents = (GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent)
                              & ~(GatewayIntents.GuildInvites | GatewayIntents.GuildScheduledEvents)
            }));

            serviceCollection.AddAutoMapper(typeof(Core.Extensions.ServiceCollectionExtensions));
            serviceCollection.AddMediatR(typeof(Commands.Extensions.ServiceCollectionExtensions));
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