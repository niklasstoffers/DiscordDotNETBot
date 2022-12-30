using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Hainz.Events.Extensions;
using Hainz.Infrastructure.Extensions;
using Hainz.Persistence.Extensions;
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
            var serverConfiguration = hostBuilder.Configuration.GetServerConfiguration();
            var botOptionsConfiguration = hostBuilder.Configuration.GetBotOptionsConfiguration();

            serviceCollection.AddCoreConfiguration(botConfiguration, 
                botOptionsConfiguration, 
                serverConfiguration);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.AddInfrastructure();

        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            var persistenceConfiguration = hostBuilder.Configuration.GetPersistenceConfigurationWithEnvVars();

            serviceCollection.AddEvents();
            serviceCollection.AddCore();
            serviceCollection.AddInfrastructure();
            serviceCollection.AddCommands();
            serviceCollection.AddPersistence(persistenceConfiguration);
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