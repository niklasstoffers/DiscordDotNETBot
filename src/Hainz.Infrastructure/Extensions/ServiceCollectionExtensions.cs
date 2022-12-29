using System.Reflection;
using Hainz.Infrastructure.Logging.Targets;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<DiscordChannelLogTarget>();

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddAutoMapper(currentAssembly);

        return serviceCollection;
    }
}