using System.Reflection;
using Discord.Commands;
using FluentValidation;
using Hainz.Commands.Config;
using Hainz.Commands.Helpers;
using Hainz.Commands.Helpers.Help;
using Hainz.Commands.Helpers.Help.Builders;
using Hainz.Commands.TypeReaders;
using Hainz.Commands.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection, CommandsConfig commandsConfiguration)
    {
        new CommandsConfigValidator().ValidateAndThrow(commandsConfiguration);
        serviceCollection.AddSingleton(commandsConfiguration);

        serviceCollection.AddSingleton(serviceProvider =>
        {
            var commandServiceConfig = new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async
            };

            var commandService = new CommandService(commandServiceConfig);

            var bootstrapper = serviceProvider.GetRequiredService<CommandModuleBootstrapper>();
            bootstrapper.Bootstrap(commandService);

            return commandService;
        });
        serviceCollection.AddSingleton<HelpRegister>();

        serviceCollection.AddTransient<HelpEntryBuilder>();
        serviceCollection.AddTransient<SectionHelpEntryBuilder>();
        serviceCollection.AddTransient<CommandHelpEntryBuilder>();
        serviceCollection.AddTransient<RootHelpEntryBuilder>();

        serviceCollection.AddTransient<HelpCommandInvocationResolver>();
        serviceCollection.AddTransient<HelpRegisterPopulator>();
        serviceCollection.AddTransient<CommandModuleBootstrapper>();
        serviceCollection.AddTransient<CommandPrefixResolver>();

        serviceCollection.AddTransient<CommandPostExecutionHandler>();
        serviceCollection.AddTransient<CommandHandler>();

        serviceCollection.AddTransient<TypeReaderBase, ActivityTypeTypeReader>();
        serviceCollection.AddTransient<TypeReaderBase, UserStatusTypeReader>();

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddMediatR(currentAssembly);

        return serviceCollection;
    }
}