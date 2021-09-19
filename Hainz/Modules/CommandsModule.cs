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
            builder.Register<CommandService>(ctx =>
            {
                var config = new CommandServiceConfig() { LogLevel = Discord.LogSeverity.Debug };
                return new CommandService(config);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<CommandHandler>().AsSelf().InstancePerLifetimeScope();
        }
    }
}