using System.Reflection;
using Discord.Commands;
using Hainz.Commands.Helpers.Help;
using Hainz.Commands.TypeReaders;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Helpers;

public sealed class CommandModuleBootstrapper
{
    private readonly IEnumerable<TypeReaderBase> _typeReaders;
    private readonly HelpRegisterPopulator _helpRegisterPopulator;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandModuleBootstrapper> _logger;

    public CommandModuleBootstrapper(IEnumerable<TypeReaderBase> typeReaders,
                                     HelpRegisterPopulator helpRegisterPopulator,
                                     IServiceProvider serviceProvider,
                                     ILogger<CommandModuleBootstrapper> logger)
    {
        _typeReaders = typeReaders;
        _helpRegisterPopulator = helpRegisterPopulator;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Bootstrap(CommandService commandService)
    {
        AddTypeReaders(commandService);
        AddCommandModules(commandService);
        
        _helpRegisterPopulator.Populate(commandService);
    }

    private void AddTypeReaders(CommandService commandService)
    {
        _logger.LogInformation("Adding type readers");
        foreach (var typeReader in _typeReaders)
        {
             _logger.LogTrace("Adding type reader {name} for type {type}", typeReader.GetType().FullName, typeReader.ForType.FullName);
            commandService.AddTypeReader(typeReader.ForType, typeReader);
        }
    }

    private void AddCommandModules(CommandService commandService)
    {
        try
        {
            _logger.LogInformation("Adding command modules");

            var assembly = Assembly.GetExecutingAssembly();
            _ = commandService.AddModulesAsync(assembly, _serviceProvider).Result;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while trying to add command modules");
        }
    }
}