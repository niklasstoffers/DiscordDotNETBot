using Hainz.Core.Services.Bot;
using Hainz.Core.Services.Guild;
using Hainz.Core.Services.Status;
using Hainz.Core.Services.Messages;
using Hainz.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Hainz.Core.Config;
using Hainz.Core.Services.Logging;
using Hainz.Core.Validation.Configuration;
using FluentValidation;
using MediatR;
using System.Reflection;
using Hainz.Core.Healthchecks.Services;

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection, BotConfig botConfig, HealthChecksConfiguration healthChecksConfig)
    {
        new BotConfigValidator().ValidateAndThrow(botConfig);
        serviceCollection.AddSingleton(botConfig);

        new HealthChecksConfigurationValidator().ValidateAndThrow(healthChecksConfig);
        serviceCollection.AddSingleton(healthChecksConfig);

        serviceCollection.AddTransient<ActivityService>();
        serviceCollection.AddTransient<StatusService>();
        serviceCollection.AddTransient<BanService>();
        serviceCollection.AddTransient<DMService>();
        serviceCollection.AddTransient<DefaultStatusService>();
        serviceCollection.AddTransient<DiscordLogAdapterService>();
        
        serviceCollection.AddTransient<ConnectionMonitorService>();
        serviceCollection.AddSingleton<UptimeMonitorService>();

        serviceCollection.AddGatewayService<GatewayConnectionService>();
        serviceCollection.AddGatewayService<DiscordChannelLoggerService>();
        serviceCollection.AddGatewayService<DatabaseHealthCheck>();
        serviceCollection.AddGatewayService<RedisHealthCheck>();

        serviceCollection.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
        {
            GatewayIntents = RequiredGatewayIntents.RequiredIntents
        }));

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddMediatR(currentAssembly);

        return serviceCollection;
    }
}