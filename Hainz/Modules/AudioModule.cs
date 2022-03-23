using Autofac;
using Hainz.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Modules
{
    public class AudioModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VoiceChannelService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AudioManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MusicBuilder>().AsSelf();
        }
    }
}
