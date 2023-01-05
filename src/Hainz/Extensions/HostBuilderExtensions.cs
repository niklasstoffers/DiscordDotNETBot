using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Hainz.Events.Extensions;
using Hainz.Infrastructure.Extensions;
using Hainz.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hainz.Helpers;

namespace Hainz.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddAppSettings(this IHostBuilder hostBuilder, bool isOptional = true)
    {
        hostBuilder.ConfigureAppConfiguration(configurationBuilder => 
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: isOptional);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.AddInfrastructure();

        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            var botConfiguration = hostBuilder.Configuration.GetBotConfiguration();
            var persistenceConfiguration = hostBuilder.Configuration.GetPersistenceConfiguration();
            var cachingConfiguration = hostBuilder.Configuration.GetCachingConfiguration();

            serviceCollection.AddEvents();
            serviceCollection.AddCore(botConfiguration);
            serviceCollection.AddInfrastructure();
            serviceCollection.AddCommands();
            serviceCollection.AddPersistence(persistenceConfiguration, cachingConfiguration);

            serviceCollection.AddTransient<HostStartup>();
            serviceCollection.AddHostedService<ApplicationHost>();
        });

        return hostBuilder;
    }
}