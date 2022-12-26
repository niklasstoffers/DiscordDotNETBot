using Autofac;
using Hainz.Commands;
using Hainz.Commands.TypeReaders;

namespace Hainz.Infrastructure.Modules;

public class CommandModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CommandHandler>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<CommandPostExecutionHandler>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterType<ActivityTypeTypeReader>()
               .As<TypeReaderBase>()
               .InstancePerDependency();

        builder.RegisterType<UserStatusTypeReader>()
               .As<TypeReaderBase>()
               .InstancePerDependency();
    }
}