using Autofac;
using Discord.WebSocket;

namespace Hainz.Modules;

internal sealed class DiscordModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new DiscordSocketClient())
               .AsSelf()
               .SingleInstance();
    }
}