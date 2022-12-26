using Autofac;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Hainz.Infrastructure.Modules;

public class DiscordModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => 
            new DiscordSocketClient(new DiscordSocketConfig() 
                {
                    GatewayIntents = (GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent)
                                  & ~(GatewayIntents.GuildInvites | GatewayIntents.GuildScheduledEvents)
                })
            )
            .AsSelf()
            .SingleInstance();

        builder.Register(ctx => 
            new CommandService(new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async
            }))
            .AsSelf()
            .SingleInstance();
    }
}