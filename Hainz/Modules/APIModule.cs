using Autofac;
using Hainz.API.Youtube;
using Hainz.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Modules
{
    public class APIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<YoutubeApiClient>(ctx =>
            {
                var config = ctx.Resolve<BotConfig>();
                return new YoutubeApiClient(config.YoutubeAPIKey);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
