using System.Reflection;
using Discord.Commands;
using FluentValidation;
using Hainz.Commands.Config;
using Hainz.Commands.TypeReaders;
using Hainz.Commands.Validation;
using Hainz.Hosting.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection, CommandsConfig commandsConfiguration)
    {
        new CommandsConfigValidator().ValidateAndThrow(commandsConfiguration);
        serviceCollection.AddSingleton(commandsConfiguration);

        serviceCollection.AddTransient<CommandPostExecutionHandler>();
        serviceCollection.AddSingleton(new CommandService(new CommandServiceConfig()
        {
            DefaultRunMode = RunMode.Async
        }));

        serviceCollection.AddTransient<TypeReaderBase, ActivityTypeTypeReader>();
        serviceCollection.AddTransient<TypeReaderBase, UserStatusTypeReader>();

        serviceCollection.AddGatewayService<CommandHandler>();

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddMediatR(currentAssembly);

        return serviceCollection;
    }
}