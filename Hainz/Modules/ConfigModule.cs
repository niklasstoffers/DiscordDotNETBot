using Autofac;
using Hainz.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Modules
{
    public class ConfigModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<BotConfig>((ctx, p) => ConfigManager.GetOrCreate()).AsSelf().InstancePerLifetimeScope();
            builder.Register<LoggerConfig>((ctx, p) => ctx.Resolve<BotConfig>().LoggerConfig).AsSelf().InstancePerLifetimeScope();
        }
    }
}
