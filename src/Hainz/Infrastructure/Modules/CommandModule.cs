using Autofac;
using Hainz.Commands;

namespace Hainz.Infrastructure.Modules;

public class CommandModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CommandHandler>()
               .AsSelf()
               .SingleInstance();
    }
}