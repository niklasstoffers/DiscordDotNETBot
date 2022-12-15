using Autofac;
using Discord.Commands;

namespace Hainz.Modules;

internal sealed class CommandModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CommandService>()
               .AsSelf()
               .SingleInstance();
    }
}