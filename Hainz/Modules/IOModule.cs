using Autofac;
using Hainz.Config;
using Hainz.IO.Input;
using Hainz.IO.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Modules
{
    public class IOModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleOutput>().AsSelf().InstancePerLifetimeScope();
            builder.Register<FileOutput>((ctx, p) =>
            {
                string path = p.Named<string>("filePath");
                return new FileOutput(path);
            }).AsSelf().InstancePerDependency();

            builder.RegisterType<ConsoleInput>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
