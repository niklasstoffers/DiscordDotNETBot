using FluentValidation;
using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Hainz.Core.Validation.Configuration;
using Hainz.Events.Extensions;
using Hainz.Infrastructure.Extensions;
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

            new BotConfigValidator().ValidateAndThrow(botConfiguration);
            new BotOptionsConfigValidator().ValidateAndThrow(botOptionsConfiguration);

            serviceCollection.AddSingleton(botConfiguration);
            serviceCollection.AddSingleton(serverConfiguration);
            serviceCollection.AddSingleton(botOptionsConfiguration);
        });

        return hostBuilder;
    }

    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            serviceCollection.AddEvents();
            serviceCollection.AddCore();
            serviceCollection.AddInfrastructure();
            serviceCollection.AddCommands();
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