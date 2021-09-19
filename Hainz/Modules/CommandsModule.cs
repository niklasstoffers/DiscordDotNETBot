using System;
using Autofac;
using Discord.Commands;
using Hainz.Commands;

namespace Hainz.Modules 
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CommandHandler>().AsSelf().InstancePerLifetimeScope();
        }
    }
}