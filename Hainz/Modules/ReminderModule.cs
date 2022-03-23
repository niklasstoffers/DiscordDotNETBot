using System;
using Autofac;
using Hainz.Util;

namespace Hainz.Modules 
{
    public class ReminderModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReminderService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ReminderParser>().AsSelf();
        }
    }
}