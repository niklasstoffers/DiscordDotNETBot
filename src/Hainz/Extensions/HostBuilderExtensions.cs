using Hainz.Commands.Extensions;
using Hainz.Core.Extensions;
using Hainz.Events.Extensions;
using Hainz.Infrastructure.Extensions;
using Hainz.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hainz.Helpers;
using MediatR;
using System.Reflection;

namespace Hainz.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddAppSettings(this IHostBuilder hostBuilder, bool isOptional = true)
    {
        hostBuilder.ConfigureAppConfiguration((hostingContext, configurationBuilder) => 
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: isOptional);

            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                configurationBuilder.AddUserSecrets<Program>(true);
            }
        });

        return hostBuilder;
    }

    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.AddInfrastructure();

        hostBuilder.ConfigureServices((hostBuilder, serviceCollection) =>
        {
            var botConfiguration = hostBuilder.Configuration.GetBotConfiguration();
            var commandsConfiguration = hostBuilder.Configuration.GetCommandsConfiguration();
            var persistenceConfiguration = hostBuilder.Configuration.GetPersistenceConfiguration();
            var cachingConfiguration = hostBuilder.Configuration.GetCachingConfiguration();
            var healthChecksConfiguration = hostBuilder.Configuration.GetHealthChecksConfiguration();

            serviceCollection.AddCore(botConfiguration, healthChecksConfiguration);
            serviceCollection.AddInfrastructure();
            serviceCollection.AddCommands(commandsConfiguration);
            serviceCollection.AddPersistence(persistenceConfiguration, cachingConfiguration);
            serviceCollection.AddEvents();

            serviceCollection.AddTransient<HostStartup>();
            serviceCollection.AddHostedService<ApplicationHost>();

            var currentAssembly = Assembly.GetExecutingAssembly();
            serviceCollection.AddMediatR(currentAssembly);
        });

        return hostBuilder;
    }
}