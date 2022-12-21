using Discord;
using Discord.WebSocket;
using Hainz.Core.Logging;
using Hainz.Core.Services.Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions 
{
    public static IServiceCollection AddCore(this IServiceCollection services) 
    {
        services.AddHostedService<Bot>();
        services.AddHostedService<DiscordLogAdapterService>();
        
        var socketConfig = new DiscordSocketConfig() 
        { 
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent 
        };

        services.AddSingleton<DiscordSocketClient>(provider => new DiscordSocketClient(socketConfig));
        services.AddTransient<DiscordStatusService>();

        return services;
    }
}