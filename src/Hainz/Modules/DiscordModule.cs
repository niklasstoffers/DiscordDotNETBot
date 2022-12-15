using Autofac;
using Discord;
using Discord.WebSocket;

namespace Hainz.Modules;

internal sealed class DiscordModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new DiscordSocketClient(new DiscordSocketConfig() { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent }))
               .AsSelf()
               .SingleInstance();
    }
}