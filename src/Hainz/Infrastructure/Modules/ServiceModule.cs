using Autofac;
using Hainz.Core.Services;
using Hainz.Core.Services.Discord;
using Hainz.Core.Services.Logging;

namespace Hainz.Infrastructure.Modules;

public sealed class ServiceModule : Module 
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