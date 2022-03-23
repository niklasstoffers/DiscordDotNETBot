using Autofac;
using Discord.WebSocket;
using Hainz.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Modules
{
    public class DiscordModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<DiscordSocketClient>((ctx, p) =>
            {
                BotConfig config = ctx.Resolve<BotConfig>();
                return new DiscordSocketClient(new DiscordSocketConfig()
                {
#if RELEASE
                    LogLevel = Discord.LogSeverity.Info
#elif DEBUG
                    LogLevel = Discord.LogSeverity.Debug
#endif
                });
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
