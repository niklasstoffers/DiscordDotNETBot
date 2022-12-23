using Autofac;
using Hainz.Services.Discord;
using Hainz.Services.Logging;

namespace Hainz.Infrastructure.Modules;

public class ServiceModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DiscordStatusService>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<DiscordActivityService>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<DiscordLogAdapterService>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<DiscordChannelLoggerService>()
               .AsSelf()
               .SingleInstance();
    }
}