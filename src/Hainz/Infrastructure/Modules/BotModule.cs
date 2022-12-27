using Autofac;

namespace Hainz.Infrastructure.Modules;

public sealed class BotModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Bot>()
               .AsSelf()
               .SingleInstance();
    }
}