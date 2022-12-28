using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.TypeReaders;
using Hainz.Events.Notifications.Messages;
using Hainz.Hosting;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

public sealed class CommandHandler : GatewayServiceBase, INotificationHandler<MessageReceived>
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IEnumerable<TypeReaderBase> _typeReaders;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(DiscordSocketClient client, 
                          CommandService commands,
                          IEnumerable<TypeReaderBase> typeReaders,
                          IServiceProvider serviceProvider,
                          ILogger<CommandHandler> logger)
    {
        _client = client;
        _commandService = commands;
        _typeReaders = typeReaders;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task SetupAsync()
    {
        _logger.LogTrace("Adding type readers");
        AddTypeReaders();

        try 
        {
            _logger.LogTrace("Adding command modules");
            await _commandService.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), 
                                                  services: _serviceProvider);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error while trying to add command modules");
        }
    }

    private void AddTypeReaders()
    {
        foreach (var typeReader in _typeReaders)
        {
            _logger.LogTrace("Adding type reader {name} for type {type}", typeReader.GetType().FullName, typeReader.ForType.FullName);
            _commandService.AddTypeReader(typeReader.ForType, typeReader);
        }
    }

    public async Task Handle(MessageReceived notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        _logger.LogTrace("Received message \"{message}\" from \"{user}\"", message.Content, message.Author.Username);

        if (message is not SocketUserMessage userMessage)
            return;

        int argPos = 0;

        if (!(userMessage.HasCharPrefix('!', ref argPos) || 
            userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            userMessage.Author.IsBot)
            return;

        _logger.LogInformation("Received command \"{command}\" from \"{user}\"", message.Content, message.Author.Username);
        var context = new SocketCommandContext(_client, userMessage);

        try 
        {
            _logger.LogTrace("Executing command");
            await _commandService.ExecuteAsync(
                context: context, 
                argPos: argPos,
                services: _serviceProvider);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Exception during command execution");
        }
    }
}