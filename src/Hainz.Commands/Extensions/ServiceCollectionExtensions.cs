using Discord.Commands;
using Hainz.Commands.TypeReaders;
using Hainz.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<CommandPostExecutionHandler>();
        serviceCollection.AddSingleton(new CommandService(new CommandServiceConfig()
        {
            DefaultRunMode = RunMode.Async
        }));

        serviceCollection.AddTransient<TypeReaderBase, ActivityTypeTypeReader>();
        serviceCollection.AddTransient<TypeReaderBase, UserStatusTypeReader>();

        serviceCollection.AddGatewayService<CommandHandler>();

        return serviceCollection;
    }
}