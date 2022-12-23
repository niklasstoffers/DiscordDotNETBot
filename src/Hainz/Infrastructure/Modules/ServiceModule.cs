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
               .InstancePerDependency();

        builder.RegisterType<DiscordActivityService>()
               .AsSelf()
               .InstancePerDependency();

        builder.RegisterType<DiscordLogAdapterService>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<DiscordChannelLoggerService>()
               .AsSelf()
               .SingleInstance();
    }
}