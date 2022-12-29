using Hainz.Core.Services.Bot;
using Hainz.Core.Services.Guild;
using Hainz.Core.Services.Status;
using Hainz.Core.Services.Messages;
using Hainz.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Hainz.Core.Config.Bot;
using Hainz.Core.Services.Logging;
using Hainz.Core.Config.BotOptions;
using Hainz.Core.Config.Server;
using Hainz.Core.Validation.Configuration;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ActivityService>();
        serviceCollection.AddTransient<StatusService>();
        serviceCollection.AddTransient<BanService>();
        serviceCollection.AddTransient<DMService>();
        serviceCollection.AddTransient<DefaultStatusService>();
        serviceCollection.AddTransient<DiscordLogAdapterService>();

        serviceCollection.AddGatewayService<GatewayConnectionService>();
        serviceCollection.AddGatewayService<DiscordChannelLoggerService>();

        serviceCollection.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
        {
            GatewayIntents = RequiredGatewayIntents.RequiredIntents
        }));

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddMediatR(currentAssembly);

        return serviceCollection;
    }

    public static IServiceCollection AddCoreConfiguration(this IServiceCollection serviceCollection,
                                                          BotConfig botConfig,
                                                          BotOptionsConfig botOptionsConfig,
                                                          ServerConfig serverConfig)
    {
        new BotConfigValidator().ValidateAndThrow(botConfig);
        new BotOptionsConfigValidator().ValidateAndThrow(botOptionsConfig);

        serviceCollection.AddSingleton(botConfig);
        serviceCollection.AddSingleton(botOptionsConfig);
        serviceCollection.AddSingleton(serverConfig);

        return serviceCollection;
    }
}