using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Hainz.Events.Extensions;
using Hainz.Infrastructure.Extensions;
using Hainz.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hainz.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddAppSettings(this IHostBuilder hostBuilder, bool optional = true)
    {
        hostBuilder.ConfigureAppConfiguration(configurationBuilder => 
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: optional);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddApplicationConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) => 
        {
            var botConfiguration = hostBuilder.Configuration.GetBotConfiguration();
            serviceCollection.AddCoreConfiguration(botConfiguration);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.AddInfrastructure();

        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            var persistenceConfiguration = hostBuilder.Configuration.GetPersistenceConfigurationWithEnvironmentVars();
            var cachingConfiguration = hostBuilder.Configuration.GetCachingConfigurationWithEnvironmentVars();

            serviceCollection.AddEvents();
            serviceCollection.AddCore();
            serviceCollection.AddInfrastructure();
            serviceCollection.AddCommands();
            serviceCollection.AddPersistence(persistenceConfiguration, cachingConfiguration);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddApplicationHost(this IHostBuilder hostBuilder) 
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) => 
        {
            serviceCollection.AddHostedService<ApplicationHost>();
        });

        return hostBuilder;
    }
}