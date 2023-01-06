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

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection, BotConfig botConfig)
    {
        new BotConfigValidator().ValidateAndThrow(botConfig);
        serviceCollection.AddSingleton(botConfig);

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
}