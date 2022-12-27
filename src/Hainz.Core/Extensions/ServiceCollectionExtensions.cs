using Hainz.Core.Services;
using Hainz.Core.Services.Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<DiscordActivityService>();
        serviceCollection.AddTransient<DiscordStatusService>();

        return serviceCollection;
    }
}