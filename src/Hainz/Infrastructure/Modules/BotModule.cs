using Autofac;

namespace Hainz.Infrastructure.Modules;

public class BotModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Bot>()
               .AsSelf()
               .SingleInstance();
    }
}