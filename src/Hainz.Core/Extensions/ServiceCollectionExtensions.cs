using Hainz.Core.Services.Bot;
using Hainz.Core.Services.Discord;
using Hainz.Core.Services.User;
using Hainz.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<DiscordActivityService>();
        serviceCollection.AddTransient<DiscordStatusService>();
        serviceCollection.AddTransient<BanService>();

        serviceCollection.AddSingleton<DefaultStatusService>();
        serviceCollection.AddGatewayService<GatewayConnectionService>();

        return serviceCollection;
    }
}