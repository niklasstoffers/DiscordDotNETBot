using Autofac;
using Hainz.Logging.Targets;

namespace Hainz.Infrastructure.Modules;

public sealed class LogModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DiscordChannelLogTarget>()
               .AsSelf()
               .InstancePerDependency();
    }
}