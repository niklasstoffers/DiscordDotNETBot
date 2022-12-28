using Hainz.Hosting.Extensions;
using Hainz.Logging.NLog.Targets;
using Hainz.Logging.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggingServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGatewayService<DiscordChannelLoggerService>();
        serviceCollection.AddSingleton<DiscordLogAdapterService>();
        serviceCollection.AddSingleton<DiscordChannelLogTarget>();

        return serviceCollection;
    }
}