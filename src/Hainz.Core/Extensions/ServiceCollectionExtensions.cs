using Hainz.Core.Services.Bot;
using Hainz.Core.Services.Guild;
using Hainz.Core.Services.Status;
using Hainz.Core.Services.Messages;
using Hainz.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Hainz.Core.Config.Bot;
using Hainz.Core.Services.Logging;

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

        return serviceCollection;
    }
}