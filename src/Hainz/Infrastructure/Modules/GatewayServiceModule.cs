using Autofac;
using Hainz.Commands;
using Hainz.Services;
using Hainz.Services.Logging;

namespace Hainz.Infrastructure.Modules;

public sealed class GatewayServiceModule : Module 
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GatewayServiceHost<CommandHandler>>()
               .As<IGatewayServiceHost<IGatewayService>>()
               .SingleInstance();

        builder.RegisterType<GatewayServiceHost<DiscordChannelLoggerService>>()
               .As<IGatewayServiceHost<IGatewayService>>()
               .SingleInstance();

        builder.RegisterType<GatewayServiceHost<DiscordLogAdapterService>>()
               .As<IGatewayServiceHost<IGatewayService>>()
               .SingleInstance();

       builder.RegisterType<GatewayServiceHost<CommandPostExecutionHandler>>()
              .As<IGatewayServiceHost<IGatewayService>>()
              .SingleInstance();
    }
}