using Autofac;
using Hainz.Logging.Targets;

namespace Hainz.Infrastructure.Modules;

public class LogModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DiscordChannelLogTarget>()
               .AsSelf()
               .InstancePerDependency();
    }
}