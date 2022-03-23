using Autofac;
using Hainz.Config;
using Hainz.IO;
using Hainz.IO.Outputs;
using Hainz.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Modules
{
    public class LogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {   
            builder.Register<DiscordLogger>(ctx => new DiscordLogger(ResolveOutput(ctx))).AsSelf().InstancePerLifetimeScope();
            builder.Register<Logger>(ctx => new Logger(ResolveOutput(ctx))).AsSelf().InstancePerLifetimeScope();

            IOutput ResolveOutput(IComponentContext ctx)
            {
                LoggerConfig config = ctx.Resolve<LoggerConfig>();
                if (config.EnableFileLogging)
                    return new MultiOutput(ctx.Resolve<ConsoleOutput>(), 
                                           ctx.Resolve<FileOutput>(new NamedParameter("filePath", config.LogDirectory)));
                return ctx.Resolve<ConsoleOutput>();
            }
        }
    }
}
